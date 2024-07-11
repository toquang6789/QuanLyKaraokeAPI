using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.DetailOderService;

namespace QuanLyKaraokeAPI.Service
{
    public interface IDetaiOderSService
    {
        Task CreateDetailOderS(CreateDetaiOderServiceDTO createDetaiOderServiceDTO);
        Task UpdateDetailOderS(int id, UpdateDetailOderServiceDTO updateDetailOderServiceDTO);
        Task DeleteDetailOderS(int id);
        Task<List<DetaiOderServiceDTO>> GetS();
        Task<DetaiOderServiceDTO> GetSById(int id);
    }
    public class DetailOderSeService : IDetaiOderSService
    {
        public readonly IRepository<DetailOderService> _dtaiprepository;
        public readonly IRepository<Oders> _orepository;
        public readonly IRepository<ServiceTime> _srepository;
        public DetailOderSeService(IRepository<DetailOderService> daiprepository, IRepository<Oders> orepository, IRepository<ServiceTime> srepository)
        {
            _dtaiprepository = daiprepository;
            _orepository = orepository;
            _srepository = srepository;
        }
        public async Task CreateDetailOderS(CreateDetaiOderServiceDTO createDetaiOderServiceDTO)
        {
            var detais = new DetailOderService
            {
                OderID = createDetaiOderServiceDTO.OderID,
                ServiceID = createDetaiOderServiceDTO.ServiceID,
                Quantity = createDetaiOderServiceDTO.Quantity,
                StartTime = createDetaiOderServiceDTO.StartTime,
                EndTime = createDetaiOderServiceDTO.EndTime,
                Status = createDetaiOderServiceDTO.Status,

            };
            await _dtaiprepository.Add(detais);
        }

        public async Task DeleteDetailOderS(int id)
        {
            var delete = await _dtaiprepository.GetById(id);
            if (delete == null) throw new Exception("Null");
            await _dtaiprepository.Delete(delete);
        }

        public async Task<List<DetaiOderServiceDTO>> GetS()
        {
            var ids = await (from d in _dtaiprepository.GetAll()
                             select d.Id).ToListAsync();

            var result = new List<DetaiOderServiceDTO>();
            /// ids.ForEach(async id => result.Add(await SVInfo(id)));
            foreach (var id in ids)
            {
                var svinfo = await SVInfo(id);
                result.Add(svinfo);
            }
            return result;
        }
        public async Task<DetaiOderServiceDTO> SVInfo(int id)
        {
            var d = await _dtaiprepository.GetAll().FirstAsync(d => d.Id == id);
            var o = await _orepository.GetAll().FirstAsync(o => o.OderID == d.OderID);
            var s = await _srepository.GetAll().FirstAsync(p => p.ServiceID == d.ServiceID);

            return new DetaiOderServiceDTO
            {
                Id = d.Id,
                OderID = d.OderID,
                ServiceID = d.ServiceID,
                serviceTime = s.ServiceName,
                Quantity = d.Quantity,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                Status = d.Status,

            };
        }
        public async Task<DetaiOderServiceDTO> GetSById(int id)
        {
            var d = await _dtaiprepository.GetAll().FirstAsync(d => d.Id == id);
            var o = await _orepository.GetAll().FirstAsync(o => o.OderID == d.OderID);
            var p = await _srepository.GetAll().FirstAsync(p => p.ServiceID == d.ServiceID);
            return new DetaiOderServiceDTO
            {
                Id = d.Id,
                OderID = d.OderID,
                ServiceID = d.ServiceID,
                serviceTime = p.ServiceName,
                Quantity = d.Quantity,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                Status = d.Status,
            };
        }

        public async Task UpdateDetailOderS(int id, UpdateDetailOderServiceDTO updateDetailOderServiceDTO)
        {
            var detai = await _dtaiprepository.GetById(id);
            if (detai == null) throw new Exception("Null");
            detai.OderID = updateDetailOderServiceDTO.OderID;
            detai.ServiceID = updateDetailOderServiceDTO.ServiceID;
            detai.Quantity = updateDetailOderServiceDTO.Quantity;
            detai.StartTime = updateDetailOderServiceDTO.StartTime;
            detai.EndTime = updateDetailOderServiceDTO.EndTime;
            detai.Status = updateDetailOderServiceDTO.Status;

            await _dtaiprepository.Update(detai);
        }
    }
}
