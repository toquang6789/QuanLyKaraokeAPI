using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.Units;

namespace QuanLyKaraokeAPI.Service
{
    public interface IUnitService
    {
        Task CreateU(CreateUnitsDTO createUnitsDTO);
        Task UpdateU(int id, UpdateUnitsDTO updateUnitsDTO);
        Task DeleteU(int id);
        Task<List<UnitsDTO>> GetU();
        Task<UnitsDTO> GetUById(int id);
        // Task<List<UserDTO>> GetUserByName(string name);
    }
    public class UnitsService : IUnitService
    {
        public readonly IRepository<Units> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UnitsService(IRepository<Units> repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task CreateU(CreateUnitsDTO createUnitsDTO)
        {
            var user = new Entities.Units
            {
                UnitName = createUnitsDTO.UnitName,
                ConvertUnit = createUnitsDTO.ConvertUnit,
            };

            await _repository.Add(user);
        }

        public async Task DeleteU(int id)
        {
            var unit = await _repository.GetById(id);
            if (unit == null) throw new Exception("Null");
            await _repository.Delete(unit);
        }

        public async Task<List<UnitsDTO>> GetU()
        {
            var result = from u in _repository.GetAll()
                         select new UnitsDTO
                         {
                             UnitName = u.UnitName,
                             ConvertUnit = u.ConvertUnit,
                         };
            return await result.ToListAsync();
        }

        public async Task<UnitsDTO> GetUById(int id)
        {
            var unit = await _repository.GetById(id);
            return new UnitsDTO
            {
                UnitName = unit.UnitName,
                ConvertUnit = unit.ConvertUnit,
            };
        }

        public async Task UpdateU(int id, UpdateUnitsDTO updateUnitsDTO)
        {
            var unit = await _repository.GetById(id);
            if (unit == null) throw new Exception("Null");
            unit.UnitName = updateUnitsDTO.UnitName;
            unit.ConvertUnit = updateUnitsDTO.ConvertUnit;
            await _repository.Update(unit);
        }
    }
}
