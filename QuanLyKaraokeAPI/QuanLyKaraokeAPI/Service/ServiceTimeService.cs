using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.ServiceTime;

namespace QuanLyKaraokeAPI.Service
{
    public interface ISTimeService
    {
        Task CreateSTime(CreateServiceTimeDTO serviceTimeDTO);
        Task UpdateSTime(int id, UpdateServiceTimeDTO updateServiceTimeDTO);
        Task DeleteSTime(int id);
        Task<List<ServiceTimeDTO>> GetSTime();
        Task<ServiceTimeDTO> GetSTimeById(int id);
    }
    public class ServiceTimeService : ISTimeService
    {
        public readonly IRepository<ServiceTime> _srepository;
        public readonly IRepository<Categories> _crepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceTimeService(IRepository<ServiceTime> srepository, IWebHostEnvironment webHostEnvironment, IRepository<Categories> crepository)
        {
            _srepository = srepository;
            _webHostEnvironment = webHostEnvironment;
            _crepository = crepository;
        }

        public async Task CreateSTime(CreateServiceTimeDTO createServiceTimeDTO)
        {
            string uniqueFileName = UploadedFile(createServiceTimeDTO);
            var product = new Entities.ServiceTime
            {
                CategoriesID = createServiceTimeDTO.CategoriesID,
                ServiceName = createServiceTimeDTO.ServiceName,
                Unit = createServiceTimeDTO.Unit,
                OpenPrice = createServiceTimeDTO.OpenPrice,
                Price = createServiceTimeDTO.Price,
                Image = uniqueFileName,
                Status = createServiceTimeDTO.Status,
            };

            await _srepository.Add(product);
        }

        private string UploadedFile(CreateServiceTimeDTO model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string webRootPath = _webHostEnvironment.ContentRootPath ?? throw new ArgumentNullException(nameof(_webHostEnvironment.WebRootPath), "Web root path cannot be null.");


                string uploadsFolder = Path.Combine(webRootPath, "Image");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }



        public async Task DeleteSTime(int id)
        {
            var time = await _srepository.GetById(id);
            if (time == null) throw new Exception("Null");
            await _srepository.Delete(time);
        }

        public async Task<List<ServiceTimeDTO>> GetSTime()
        {
            var result = from u in _srepository.GetAll()
                         join c in _crepository.GetAll()
                         on u.CategoriesID equals c.CategoriesID
                         select new ServiceTimeDTO
                         {
                             ServiceID = u.ServiceID,
                             CategoriesID = u.CategoriesID,
                             CategoriesName = c.CategoryName,
                             ServiceName = u.ServiceName,
                             Unit = u.Unit,
                             OpenPrice = u.OpenPrice,
                             Price = u.Price,
                             Image = u.Image,
                             Status = u.Status,
                         };
            return await result.ToListAsync();
        }

        public async Task<ServiceTimeDTO> GetSTimeById(int id)
        {

            var s = await _srepository.GetById(id);
            //var c = await _crepository.GetById(id);

            return new ServiceTimeDTO
            {
                CategoriesID = s.CategoriesID,

                //CategoriesName = c.CategoryName,
                ServiceName = s.ServiceName,
                Unit = s.Unit,
                OpenPrice = s.OpenPrice,
                Price = s.Price,
                Image = s.Image,
                Status = s.Status,
            };
        }


        public async Task UpdateSTime(int id, UpdateServiceTimeDTO updateServiceTimeDTO)
        {
            var time = await _srepository.GetById(id);
            if (time == null) throw new Exception("Null");
            time.CategoriesID = updateServiceTimeDTO.CategoriesID;
            time.ServiceName = updateServiceTimeDTO.ServiceName;
            time.Unit = updateServiceTimeDTO.Unit;
            time.OpenPrice = updateServiceTimeDTO.OpenPrice;
            time.Price = updateServiceTimeDTO.Price;
            if (updateServiceTimeDTO.Image != null)
            {
                string uniqueFileName = UploadedFileUpadate(updateServiceTimeDTO);
                time.Image = uniqueFileName;
            }
            time.Status = updateServiceTimeDTO.Status;
            await _srepository.Update(time);
        }
        private string UploadedFileUpadate(UpdateServiceTimeDTO model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
