using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.User;

namespace QuanLyKaraokeAPI.Service
{
    public interface IUService
    {
        Task CreateUser(CreateUserDTO userDTO);
        Task UpdateUser(int id, UpdateUserDTO userDTO);
        Task DeleteUser(int id);
        Task<List<UserDTO>> GetUser();
        Task<UserDTO> GetUserById(int id);
        Task<List<UserDTO>> GetUserByName(string name);
    }
    public class UserService : IUService
    {
        public readonly IRepository<User> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(IRepository<User> repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task CreateUser(CreateUserDTO userDTO)
        {
            string uniqueFileName = UploadedFile(userDTO);
            var user = new Entities.User
            {
                fullName = userDTO.fullName,
                Email = userDTO.Email,
                Phone = userDTO.Phone,
                Address = userDTO.Address,
                Birthday = userDTO.Birthday,
                Sex = userDTO.Sex,
                Avatar = uniqueFileName,
                TypeUser = userDTO.TypeUser,
                Status = userDTO.Status,
            };

            await _repository.Add(user);
        }
        private string ConvertToBase64(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageArray);
        }
        private string UploadedFile(CreateUserDTO model)
        {
            string uniqueFileName = null;

            if (model.Avatar == null)
                return null;
            string webRootPath = _webHostEnvironment.ContentRootPath ?? throw new ArgumentNullException(nameof(_webHostEnvironment.WebRootPath), "Web root path cannot be null.");


            string uploadsFolder = Path.Combine(webRootPath, "Image");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Avatar.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Avatar.CopyTo(fileStream);
            }


            return ConvertToBase64(filePath);
        }

        //private string UploadedFile(CreateUserDTO model)
        //{
        //    string uniqueFileName = null;
        //    if (model.Avatar != null)
        //    {
        //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Avatar.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.Avatar.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}
        public async Task DeleteUser(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null) throw new Exception("Null");
            await _repository.Delete(user);
        }

        public async Task<List<UserDTO>> GetUser()
        {
            var result = from u in _repository.GetAll()
                         select new UserDTO
                         {
                             UserId = u.UserId,
                             fullName = u.fullName,
                             Email = u.Email,
                             Phone = u.Phone,
                             Address = u.Address,
                             Birthday = u.Birthday,
                             Sex = u.Sex,
                             Avatar = u.Avatar,
                             TypeUser = u.TypeUser,
                             Status = u.Status,
                         };
            return await result.ToListAsync();
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _repository.GetById(id);
            return new UserDTO
            {
                fullName = user.fullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Birthday = user.Birthday,
                Sex = user.Sex,
                Avatar = user.Avatar,
                TypeUser = user.TypeUser,
                Status = user.Status,
            };
        }

        public Task<List<UserDTO>> GetUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUser(int id, UpdateUserDTO userDTO)
        {
            var user = await _repository.GetById(id);
            if (user == null) throw new Exception("Null");
            user.fullName = userDTO.fullName;
            user.Email = userDTO.Email;
            user.Phone = userDTO.Phone;
            user.Address = userDTO.Address;
            user.Birthday = userDTO.Birthday;
            if (userDTO.Avatar != null)
            {
                string uniqueFileName = UploadedFileUpadate(userDTO);
                user.Avatar = uniqueFileName;
            }
            user.TypeUser = userDTO.TypeUser;
            user.Status = userDTO.Status;
            await _repository.Update(user);
        }

        private string UploadedFileUpadate(UpdateUserDTO model)
        {
            string uniqueFileName = null;
            if (model.Avatar == null) return null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Avatar.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Avatar.CopyTo(fileStream);
            }

            return ConvertToBase64(filePath);
        }
    }
}
