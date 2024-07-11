using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.DetailOderProduct;

namespace QuanLyKaraokeAPI.Service
{
    public interface IDetaiOderPService
    {
        Task CreateDetailOderP(CreateDetailOderProductDTO createDetailOderProductDTO);
        Task UpdateDetailOderP(int id, UpdateDetailOderProductDTO updateDetailOderProductDTO);
        Task DeleteDetailOderP(int id);
        Task<List<DetailOderProductDTO>> GetCa();
        Task<DetailOderProductDTO> GetCaById(int id);
    }
    public class DetailOderProductService : IDetaiOderPService
    {
        public readonly IRepository<DetailOderProduct> _dtaiprepository;
        public readonly IRepository<Oders> _orepository;
        public readonly IRepository<Products> _prepository;
        public readonly AppDBContext _context;
        public DetailOderProductService(AppDBContext context, IRepository<DetailOderProduct> daiprepository, IRepository<Oders> orepository, IRepository<Products> prepository)
        {
            _context = context;
            _dtaiprepository = daiprepository;
            _orepository = orepository;
            _prepository = prepository;
        }
        public async Task CreateDetailOderP(CreateDetailOderProductDTO createDetailOderProductDTO)
        {
            var existingOrder = await _context.Oders.FindAsync(createDetailOderProductDTO.OderID);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {createDetailOderProductDTO.OderID} does not exist.");
            }
            var detaiP = new DetailOderProduct
            {
                OderID = createDetailOderProductDTO.OderID,
                ProductID = createDetailOderProductDTO.ProductID,
                Quantity = createDetailOderProductDTO.Quantity,
                TimeOder = createDetailOderProductDTO.TimeOder,
                Status = createDetailOderProductDTO.Status,

            };
            await _dtaiprepository.Add(detaiP);
        }

        public async Task DeleteDetailOderP(int id)
        {
            var detaiP = await _dtaiprepository.GetById(id);
            if (detaiP == null) throw new Exception("Null");
            await _dtaiprepository.Delete(detaiP);
        }

        public async Task<List<DetailOderProductDTO>> GetCa()
        {
            var ids = await (from d in _dtaiprepository.GetAll()
                             select d.Id).ToListAsync();

            var result = new List<DetailOderProductDTO>();
            /// ids.ForEach(async id => result.Add(await SVInfo(id)));
            foreach (var id in ids)
            {
                var svinfo = await SVInfo(id);
                result.Add(svinfo);
            }
            return result;
        }
        public async Task<DetailOderProductDTO> SVInfo(int id)
        {
            var d = await _dtaiprepository.GetAll().FirstAsync(d => d.Id == id);
            var o = await _orepository.GetAll().FirstAsync(o => o.OderID == d.OderID);
            var p = await _prepository.GetAll().FirstAsync(p => p.ProductID == d.ProductID);

            return new DetailOderProductDTO
            {
                Id = d.Id,
                OderID = d.OderID,
                ProductID = d.ProductID,
                productsName = p.ProductName,
                Quantity = d.Quantity,
                TimeOder = d.TimeOder,
                Status = d.Status,

            };
        }
        public async Task<DetailOderProductDTO> GetCaById(int id)
        {
            var d = await _dtaiprepository.GetAll().FirstAsync(d => d.Id == id);
            var o = await _orepository.GetAll().FirstAsync(o => o.OderID == d.OderID);
            var p = await _prepository.GetAll().FirstAsync(p => p.ProductID == d.ProductID);
            return new DetailOderProductDTO
            {
                Id = d.Id,
                OderID = d.OderID,
                ProductID = d.ProductID,
                productsName = p.ProductName,
                Quantity = d.Quantity,
                TimeOder = d.TimeOder,
                Status = d.Status,
            };
        }

        public async Task UpdateDetailOderP(int id, UpdateDetailOderProductDTO updateDetailOderProductDTO)
        {
            var detai = await _dtaiprepository.GetById(id);
            if (detai == null) throw new Exception("Null");
            detai.OderID = updateDetailOderProductDTO.OderID;
            detai.ProductID = updateDetailOderProductDTO.ProductID;
            detai.Quantity = updateDetailOderProductDTO.Quantity;
            detai.TimeOder = updateDetailOderProductDTO.TimeOder;
            detai.Status = updateDetailOderProductDTO.Status;

            await _dtaiprepository.Update(detai);
        }
    }
}
