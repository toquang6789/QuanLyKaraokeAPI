using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.Table;

namespace QuanLyKaraokeAPI.Service
{
    public interface ITService
    {
        Task CreateTable(CreateTableDTO createTableDTO);
        Task UpdateTable(int id, UpdateTableDTO updateTableDTO);
        Task DeleteTable(int id);
        Task<List<TableDTO>> GetTable();
        Task<TableDTO> GetTableById(int id);



        Task<bool> UpdateTableStatusAsync(int id, int status);
        Task<bool> ReserveTable(int tableId);
    }
    public class TableService : ITService
    {
        public readonly IRepository<Table> _tableRepository;
        public readonly AppDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TableService(IRepository<Table> tableRepository, IWebHostEnvironment webHostEnvironment, AppDBContext dbContext)
        {
            _tableRepository = tableRepository;
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }
        public async Task CreateTable(CreateTableDTO createTableDTO)
        {
            string uniqueFileName = UploadedFile(createTableDTO);
            var table = new Table
            {
                TabelName = createTableDTO.CreateTabelName,
                Image = uniqueFileName,
                Status = createTableDTO.Status,
            };
            await _tableRepository.Add(table);
        }

        private string ConvertToBase64(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageArray);
        }

        private string UploadedFile(CreateTableDTO model)
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
        public async Task DeleteTable(int id)
        {
            var table = await _tableRepository.GetById(id);
            if (table == null) throw new Exception("Null");
            await _tableRepository.Delete(table);
        }

        public async Task<List<TableDTO>> GetTable()
        {
            var resuft = from t in _tableRepository.GetAll()
                         select new TableDTO
                         {
                             TableID = t.TableID,
                             TabelName = t.TabelName,
                             Image = t.Image,
                             Status = t.Status,
                         };
            return await resuft.ToListAsync();
        }

        public async Task<TableDTO> GetTableById(int id)
        {
            var table = await _tableRepository.GetById(id);
            return new TableDTO
            {
                TableID = table.TableID,
                TabelName = table.TabelName,
                Image = table.Image,
                Status = table.Status,
            };
        }

        public async Task UpdateTable(int id, UpdateTableDTO updateTableDTO)
        {
            var table = await _tableRepository.GetById(id);
            if (table == null) throw new Exception("Null");
            table.TabelName = updateTableDTO.CreateTabelName;
            if (updateTableDTO.Image != null)
            {
                string uniqueFileName = UploadedFileUpadate(updateTableDTO);
                table.Image = uniqueFileName;
            }
            table.Status = updateTableDTO.Status;
            await _tableRepository.Update(table);
        }
        private string UploadedFileUpadate(UpdateTableDTO model)
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

        public async Task<bool> UpdateTableStatusAsync(int id, int status)
        {
            var table = await _dbContext.Tables.FindAsync(id);
            if (table == null) return false;

            table.Status = status;
            _dbContext.Tables.Update(table);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReserveTable(int tableId)
        {
            var table = await _dbContext.Tables.FindAsync(tableId);

            if (table == null)
            {
                throw new InvalidOperationException("Table not found");
            }

            // Kiểm tra trạng thái của bàn 
            if (table.Status != TableStatusConstants.Available)
            {
                // Nếu bàn không phải là Available, từ chối yêu cầu đặt bàn
                return false;
            }

            // Cập nhật trạng thái của bàn và lưu vào cơ sở dữ liệu
            table.Status = TableStatusConstants.Booked;
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
