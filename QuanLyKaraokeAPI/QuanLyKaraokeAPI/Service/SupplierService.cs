using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.ModelDTO.Supplier;

namespace QuanLyKaraokeAPI.Service
{
    public interface ISuppService
    {
        Task CreateSupp(CreateSupplierDTO createSupplierDTO);
        Task UpdateSupp(int id, UpdateSupplierDTO updateSupplierDTO);
        Task DeleteSupp(int id);
        Task<List<SupplierDTO>> GetSupp();
        Task<SupplierDTO> GetSuppById(int id);
    }
    public class SupplierService : ISuppService
    {
        public readonly IRepository<Supplier> _SuppRepository;
        public SupplierService(IRepository<Supplier> suppRepository)
        {
            _SuppRepository = suppRepository;
        }

        public async Task CreateSupp(CreateSupplierDTO createSupplierDTO)
        {
            var supp = new Supplier
            {
                SupplierName = createSupplierDTO.SupplierName,
                Phone = createSupplierDTO.Phone,
                Email = createSupplierDTO.Email,
                Address = createSupplierDTO.Address,
            };
            await _SuppRepository.Add(supp);
        }

        public async Task DeleteSupp(int id)
        {
            var supper = await _SuppRepository.GetById(id);
            if (supper == null) throw new Exception("Null");
            await _SuppRepository.Delete(supper);
        }

        public async Task<List<SupplierDTO>> GetSupp()
        {
            var result = from s in _SuppRepository.GetAll()
                         select new SupplierDTO
                         {
                             SupplierID = s.SupplierID,
                             SupplierName = s.SupplierName,
                             Phone = s.Phone,
                             Email = s.Email,
                             Address = s.Address,
                         };
            return await result.ToListAsync();
        }

        public async Task<SupplierDTO> GetSuppById(int id)
        {
            var supper = await _SuppRepository.GetById(id);
            return new SupplierDTO
            {
                SupplierID = supper.SupplierID,
                SupplierName = supper.SupplierName,
                Phone = supper.Phone,
                Email = supper.Email,
                Address = supper.Address,
            };
        }

        public async Task UpdateSupp(int id, UpdateSupplierDTO updateSupplierDTO)
        {
            var supp = await _SuppRepository.GetById(id);
            if (supp == null) throw new Exception("Null");
            supp.SupplierName = updateSupplierDTO.SupplierName;
            supp.Phone = updateSupplierDTO.Phone;
            supp.Email = updateSupplierDTO.Email;
            supp.Address = updateSupplierDTO.Address;
            await _SuppRepository.Update(supp);
        }
    }
}
