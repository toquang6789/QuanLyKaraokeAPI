using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.Categories;

namespace QuanLyKaraokeAPI.Service
{
    public interface ICService
    {
        Task CreateCa(CreateCategoryDTO createCategoryDTO);
        Task UpdateCa(int id, UpdateCategoryDTO updateCategoryDTO);
        Task DeleteCa(int id);
        Task<List<CategoryDTO>> GetCa();
        Task<CategoryDTO> GetCaById(int id);

    }
    public class CategoryService : ICService
    {
        public readonly IRepository<Categories> _cateRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDBContext _context;
        public CategoryService(IRepository<Categories> cateRepository, IWebHostEnvironment webHostEnvironment)
        {
            _cateRepository = cateRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task CreateCa(CreateCategoryDTO createCategoryDTO)
        {
            string uniqueFileName = UploadedFile(createCategoryDTO);

            var cate = new Categories
            {

                CategoryName = createCategoryDTO.CategoryName,
                Image = uniqueFileName,

            };
            await _cateRepository.Add(cate);
        }
        private string ConvertToBase64(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageArray);
        }
        private string UploadedFile(CreateCategoryDTO model)
        {
            string uniqueFileName = null;

            if (model.Image == null) return null;

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

            return ConvertToBase64(filePath);
        }
        public async Task DeleteCa(int id)
        {
            var cate = await _cateRepository.GetById(id);
            if (cate == null) throw new Exception("Null");
            await _cateRepository.Delete(cate);
        }

        public async Task<List<CategoryDTO>> GetCa()
        {
            var resuft = from c in _cateRepository.GetAll()
                         select new CategoryDTO
                         {
                             CategoriesID = c.CategoriesID,
                             CategoryName = c.CategoryName,
                             Image = c.Image,
                         };
            return await resuft.ToListAsync();
        }

        public async Task<CategoryDTO> GetCaById(int id)
        {
            var cate = await _cateRepository.GetById(id);
            return new CategoryDTO
            {
                CategoryName = cate.CategoryName,
                Image = cate.Image,
            };
        }

        public async Task UpdateCa(int id, UpdateCategoryDTO updateCategoryDTO)
        {
            var cate = await _cateRepository.GetById(id);
            if (cate == null) throw new Exception("Null");
            cate.CategoryName = updateCategoryDTO.CategoryName;
            if (updateCategoryDTO.Image != null)
            {
                string uniqueFileName = UploadedFileUpadate(updateCategoryDTO);
                cate.Image = uniqueFileName;
            }

            await _cateRepository.Update(cate);
        }
        private string UploadedFileUpadate(UpdateCategoryDTO model)
        {
            string uniqueFileName = null;
            if (model.Image == null) return null;

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

            return ConvertToBase64(filePath);
        }
    }
}
