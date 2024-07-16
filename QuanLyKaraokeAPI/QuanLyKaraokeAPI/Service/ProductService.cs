using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.Products;

namespace QuanLyKaraokeAPI.Service
{
    public interface IProService
    {
        Task CreateP(CreateProductDTO createProductDTO);
        Task UpdateP(int id, UpdateProductDTO updateProductDTO);
        Task Deletep(int id);
        Task<List<ProductDTO>> GetP();
        Task<ProductDTO> GetPById(int id);
        // Task<List<UserDTO>> GetUserByName(string name);
    }

    public class ProductService : IProService
    {
        public readonly IRepository<Products> _prepository;
        public readonly IRepository<Categories> _carepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IRepository<Products> repository, IWebHostEnvironment webHostEnvironment, IRepository<Categories> crepository)
        {
            _prepository = repository;
            _webHostEnvironment = webHostEnvironment;
            _carepository = crepository;
        }
        public async Task CreateP(CreateProductDTO createProductDTO)
        {
            string uniqueFileName = UploadedFile(createProductDTO);
            var product = new Entities.Products
            {
                CategoriesID = createProductDTO.CategoriesID,
                ProductName = createProductDTO.ProductName,
                Unit = createProductDTO.Unit,
                CostPrite = createProductDTO.CostPrite,
                Price = createProductDTO.Price,
                Quantity = createProductDTO.Quantity,
                Image = uniqueFileName,
                Status = createProductDTO.Status,
            };

            await _prepository.Add(product);
        }
        private string ConvertToBase64(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageArray);
        }
        private string UploadedFile(CreateProductDTO model)
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
        public async Task Deletep(int id)
        {
            var product = await _prepository.GetById(id);
            if (product == null) throw new Exception("Null");
            await _prepository.Delete(product);
        }

        public async Task<List<ProductDTO>> GetP()
        {
            var result = from u in _prepository.GetAll()
                         join c in _carepository.GetAll()
                         on u.CategoriesID equals c.CategoriesID
                         select new ProductDTO
                         {
                             ProductID = u.ProductID,
                             CategoriesID = u.CategoriesID,
                             CategoriesName = c.CategoryName,
                             ProductName = u.ProductName,
                             Unit = u.Unit,
                             CostPrite = u.CostPrite,
                             Price = u.Price,
                             Quantity = u.Quantity,
                             Image = u.Image,
                             Status = u.Status,
                         };
            return await result.ToListAsync();
        }

        public async Task<ProductDTO> GetPById(int id)
        {
            var product = await _prepository.GetById(id);
            var c = await _carepository.GetById(id);
            return new ProductDTO
            {
                ProductID= product.ProductID,
                CategoriesID = product.CategoriesID,
                CategoriesName = c.CategoryName,
                ProductName = product.ProductName,
                Unit = product.Unit,
                CostPrite = product.CostPrite,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                Status = product.Status,
            };
        }

        public async Task UpdateP(int id, UpdateProductDTO updateProductDTO)
        {
            var product = await _prepository.GetById(id);
            if (product == null) throw new Exception("Null");
            product.CategoriesID = updateProductDTO.CategoriesID;
            product.ProductName = updateProductDTO.ProductName;
            product.Unit = updateProductDTO.Unit;
            product.CostPrite = updateProductDTO.CostPrite;
            product.Price = updateProductDTO.Price;
            product.Quantity = updateProductDTO.Quantity;
            if (updateProductDTO.Image != null)
            {
                string uniqueFileName = UploadedFileUpadate(updateProductDTO);
                product.Image = uniqueFileName;
            }
            product.Status = updateProductDTO.Status;
            await _prepository.Update(product);
        }
        private string UploadedFileUpadate(UpdateProductDTO model)
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
