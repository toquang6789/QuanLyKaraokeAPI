using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.ImportProducts;

namespace QuanLyKaraokeAPI.Service
{
    public interface IIPService
    {
        Task CreateIP(CreateImportProductDTO createImportProductDTO);
        Task UpdateIP(int id, UpdateImportProductDTO updateImportProductDTO);
        Task DeleteIP(int id);
        Task<List<ImportProductDTO>> GetIP();
        Task<ImportProductDTO> GetIPById(int id);
    }
    public class ImportProductService : IIPService
    {
        public readonly IRepository<ImportProducts> _iprepository;
        public readonly IRepository<User> _urepository;
        public readonly IRepository<Supplier> _srepository;


        public ImportProductService(IRepository<ImportProducts> iprepository, IRepository<User> urepository, IRepository<Supplier> srepository)
        {
            _iprepository = iprepository;
            _srepository = srepository;
            _urepository = urepository;
        }
        public async Task CreateIP(CreateImportProductDTO createImportProductDTO)
        {
            var ip = new Entities.ImportProducts
            {
                UserId = createImportProductDTO.UserId,
                SupplierID = createImportProductDTO.SupplierID,
                TotalMoney = createImportProductDTO.TotalMoney,
                Status = createImportProductDTO.Status,
                CreateDate = createImportProductDTO.CreateDate,
            };
            await _iprepository.Add(ip);
        }

        public async Task DeleteIP(int id)
        {
            var imp = await _iprepository.GetById(id);
            if (imp == null) throw new Exception("Null");
            await _iprepository.Delete(imp);
        }

        public async Task<List<ImportProductDTO>> GetIP()
        {
            var ids = await (from o in _iprepository.GetAll()
                             select o.IdImport).ToListAsync();

            var result = new List<ImportProductDTO>();
            /// ids.ForEach(async id => result.Add(await SVInfo(id)));
            foreach (var id in ids)
            {
                var svinfo = await SVInfo(id);
                result.Add(svinfo);
            }
            return result;
        }
        public async Task<ImportProductDTO> SVInfo(int id)
        {
            var ip = await _iprepository.GetAll().FirstAsync(ip => ip.IdImport == id);
            var s = await _srepository.GetAll().FirstAsync(s => s.SupplierID == ip.SupplierID);
            var u = await _urepository.GetAll().FirstAsync(u => u.UserId == ip.UserId);

            return new ImportProductDTO
            {
                IdImport = ip.IdImport,
                UserId = ip.UserId,
                UserName = u.fullName,
                SupplierID = ip.SupplierID,
                SupplierName = s.SupplierName,
                TotalMoney = ip.TotalMoney,
                CreateDate = ip.CreateDate,
                Status = ip.Status,
            };
        }
        public async Task<ImportProductDTO> GetIPById(int id)
        {
            var detai = await _iprepository.GetById(id);
            var s = await _srepository.GetById(id);
            var u = await _urepository.GetById(id);
            return new ImportProductDTO
            {
                IdImport = detai.IdImport,
                UserId = detai.UserId,
                UserName = u.fullName,
                SupplierID = detai.SupplierID,
                SupplierName = s.SupplierName,
                TotalMoney = detai.TotalMoney,
                Status = detai.Status,
                CreateDate = detai.CreateDate,

            };
        }

        public async Task UpdateIP(int id, UpdateImportProductDTO updateImportProductDTO)
        {
            var detai = await _iprepository.GetById(id);
            if (detai == null) throw new Exception("Null");
            detai.UserId = updateImportProductDTO.UserId;
            detai.SupplierID = updateImportProductDTO.SupplierID;
            detai.TotalMoney = updateImportProductDTO.TotalMoney;
            detai.CreateDate = updateImportProductDTO.CreateDate;
            detai.Status = updateImportProductDTO.Status;
            await _iprepository.Update(detai);
        }
    }
}
