using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.DetailImports;

namespace QuanLyKaraokeAPI.Service
{
    public interface IDetaiImportService
    {
        Task CreateDetailI(CreateDetailImportDTO createDetailImportDTO);
        Task UpdateDetailI(int id, UpdateDetailImportDTO updateDetailImportDTO);
        Task DeleteDetailI(int id);
        Task<List<DetaiImportsDTO>> GetDI();
        Task<DetaiImportsDTO> GetDIById(int id);
    }
    public class DetailImportService : IDetaiImportService
    {
        public readonly IRepository<DetailImports> _dtaiprepository;
        public readonly IRepository<Products> _prepository;
        public readonly IRepository<Units> _urepository;
        public readonly IRepository<ImportProducts> _importproducts;
        public DetailImportService(IRepository<DetailImports> dtaiprepository, IRepository<Products> prepository, IRepository<Units> urepository, IRepository<ImportProducts> importproducts)
        {
            _dtaiprepository = dtaiprepository;
            _prepository = prepository;
            _urepository = urepository;
            _importproducts = importproducts;
        }
        public async Task CreateDetailI(CreateDetailImportDTO createDetailImportDTO)
        {
            var detaiP = new DetailImports
            {
                IdImport = createDetailImportDTO.IdImport,
                ProductID = createDetailImportDTO.ProductID,
                Quantity = createDetailImportDTO.Quantity,
                UnitID = createDetailImportDTO.UnitID,
                Monney = createDetailImportDTO.Monney,

            };
            await _dtaiprepository.Add(detaiP);
        }

        public async Task DeleteDetailI(int id)
        {
            var detaiI = await _dtaiprepository.GetById(id);
            if (detaiI == null) throw new Exception("Null");
            await _dtaiprepository.Delete(detaiI);
        }

        public async Task<List<DetaiImportsDTO>> GetDI()
        {
            var ids = await (from d in _dtaiprepository.GetAll()
                             select d.Id).ToListAsync();

            var result = new List<DetaiImportsDTO>();
            /// ids.ForEach(async id => result.Add(await SVInfo(id)));
            foreach (var id in ids)
            {
                var svinfo = await SVInfo(id);
                result.Add(svinfo);
            }
            return result;
        }

        public async Task<DetaiImportsDTO> GetDIById(int id)
        {
            var d = await _dtaiprepository.GetAll().FirstAsync(d => d.Id == id);
            var u = await _urepository.GetAll().FirstAsync(u => u.UnitID == d.UnitID);
            var p = await _prepository.GetAll().FirstAsync(p => p.ProductID == d.ProductID);
            var ip = await _importproducts.GetAll().FirstAsync(ip => ip.IdImport == d.IdImport);

            return new DetaiImportsDTO
            {
                Id = d.Id,
                IdImport = d.IdImport,
                ProductID = d.ProductID,
                Products = p.ProductName,
                Quantity = d.Quantity,
                UnitID = d.UnitID,
                Units = u.UnitName,
                Monney = d.Monney,

            };
        }
        public async Task<DetaiImportsDTO> SVInfo(int id)
        {
            var d = await _dtaiprepository.GetAll().FirstAsync(d => d.Id == id);
            var u = await _urepository.GetAll().FirstAsync(u => u.UnitID == d.UnitID);
            var p = await _prepository.GetAll().FirstAsync(p => p.ProductID == d.ProductID);
            var ip = await _importproducts.GetAll().FirstAsync(ip => ip.IdImport == d.IdImport);

            return new DetaiImportsDTO
            {
                Id = d.Id,
                IdImport = d.IdImport,
                ProductID = d.ProductID,
                Products = p.ProductName,
                Quantity = d.Quantity,
                UnitID = d.UnitID,
                Units = u.UnitName,
                Monney = d.Monney,
            };
        }
        public async Task UpdateDetailI(int id, UpdateDetailImportDTO updateDetailImportDTO)
        {
            var detai = await _dtaiprepository.GetById(id);
            if (detai == null) throw new Exception("Null");
            detai.IdImport = updateDetailImportDTO.IdImport;
            detai.ProductID = updateDetailImportDTO.ProductID;
            detai.Quantity = updateDetailImportDTO.Quantity;
            detai.UnitID = updateDetailImportDTO.UnitID;
            detai.Monney = updateDetailImportDTO.Monney;

            await _dtaiprepository.Update(detai);
        }
    }
}
