using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.Entities.Login;
using QuanLyKaraokeAPI.ModelDTO.Oder;
using QuanLyKaraokeAPI.ModelDTO.Table;

namespace QuanLyKaraokeAPI.Service
{
    public interface IOService
    {
        Task CreateO(CreateOderDTO createOderDTO);
        Task UpdateO(int id, UpdateOderDTO updateOderDTO);
        Task DeleteO(int id);
        Task<List<OderDTO>> GetO();
        Task<OderDTO> GetOById(int id);

        Task<bool> MergeOrdersAsync(int sourceOrderId, int targetOrderId);
        Task<bool> AddOrderAsync(CreateOderDTO createOrderDTO);
        Task<bool> UpdateOrderStatusOnPayment(int orderId);
    }
    public class OderService : IOService
    {
        public readonly IRepository<Table> _trepository;
        public readonly IRepository<User> _urepository;
        public readonly IRepository<Oders> _orepository;
        public readonly IRepository<Account> _arepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly AppDBContext _dbContext;

        public OderService(AppDBContext dbContext, IRepository<Account> arepository, IRepository<Table> trepository, IWebHostEnvironment webHostEnvironment, IRepository<User> urepository, IRepository<Oders> orepository)
        {
            _dbContext = dbContext;
            _arepository = arepository;
            _orepository = orepository;
            _trepository = trepository;
            _urepository = urepository;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task CreateO(CreateOderDTO createOderDTO)
        {
            Console.WriteLine($"Checking AccountID: {createOderDTO.AccountID}");
            var accountExists = await _dbContext.Accounts.AnyAsync(a => a.AccountID == createOderDTO.AccountID);
            if (!accountExists)
            {
                throw new Exception("AccountID does not exist.");
            }

            var oder = new Entities.Oders
            {
                AccountID = createOderDTO.AccountID,
                UserId = createOderDTO.UserId,
                TableID = createOderDTO.TableID,
                TotalMoney = createOderDTO.TotalMoney,
                Status = createOderDTO.Status,
                CreateDate = createOderDTO.CreateDate,
                EndDate = createOderDTO.EndDate,
            };

            await _orepository.Add(oder);
        }

        public async Task DeleteO(int id)
        {
            var oder = await _orepository.GetById(id);
            if (oder == null) throw new Exception("Null");
            await _orepository.Delete(oder);
        }

        public async Task<List<OderDTO>> GetO()
        {
            var ids = await (from o in _orepository.GetAll()
                             select o.OderID).ToListAsync();

            var result = new List<OderDTO>();
            /// ids.ForEach(async id => result.Add(await SVInfo(id)));
            foreach (var id in ids)
            {
                var svinfo = await SVInfo(id);
                result.Add(svinfo);
            }
            return result;
        }
        public async Task<OderDTO> SVInfo(int id)
        {
            var o = await _orepository.GetAll().FirstAsync(o => o.OderID == id);
            var a = await _arepository.GetAll().FirstAsync(a => a.AccountID == o.AccountID);
            var u = await _urepository.GetAll().FirstAsync(u => u.UserId == o.UserId);
            var t = await _trepository.GetAll().FirstAsync(t => t.TableID == o.TableID);
            return new OderDTO
            {
                OderID = o.OderID,
                AccountID = o.AccountID,
                AccountName = a.AccountName,
                UserId = o.UserId,
                UserName = u.fullName,
                TableID = o.TableID,
                TableName = t.TabelName,
                TotalMoney = o.TotalMoney,
                Status = o.Status,
                CreateDate = o.CreateDate,
                EndDate = o.EndDate,
            };
        }
        public async Task<OderDTO> GetOById(int id)
        {
            var detai = await _orepository.GetById(id);
            var a = await _arepository.GetById(id);
            var u = await _urepository.GetById(id);
            var t = await _trepository.GetById(id);
            return new OderDTO
            {
                OderID = detai.OderID,
                AccountID = detai.AccountID,
                AccountName = a.AccountName,
                UserId = detai.UserId,
                UserName = u.fullName,
                TableID = detai.TableID,
                TableName = t.TabelName,
                TotalMoney = detai.TotalMoney,
                Status = detai.Status,
                CreateDate = detai.CreateDate,
                EndDate = detai.EndDate,
            };
        }

        public async Task UpdateO(int id, UpdateOderDTO updateOderDTO)
        {
            var detai = await _orepository.GetById(id);
            if (detai == null) throw new Exception("Null");
            detai.AccountID = updateOderDTO.AccountID;
            detai.UserId = updateOderDTO.UserId;
            detai.TableID = updateOderDTO.TableID;
            detai.TotalMoney = updateOderDTO.TotalMoney;
            detai.Status = updateOderDTO.Status;
            detai.CreateDate = updateOderDTO.CreateDate;
            detai.EndDate = updateOderDTO.EndDate;
            await _orepository.Update(detai);
        }

        public async Task<bool> MergeOrdersAsync(int sourceOrderId, int targetOrderId)
        {
            var sourceOrder = await _dbContext.Oders.FindAsync(sourceOrderId);
            var targetOrder = await _dbContext.Oders.FindAsync(targetOrderId);

            if (sourceOrder == null || targetOrder == null) return false;

            // Merge logic: Chuyển sản phẩm từ sourceOrder sang targetOrder, sau đó xóa sourceOrder
            var sourceOrderDetails = _dbContext.OrdersProduct.Where(d => d.OderID == sourceOrderId).ToList();
            foreach (var detail in sourceOrderDetails)
            {
                detail.OderID = targetOrderId;
                _dbContext.OrdersProduct.Update(detail);
            }

            _dbContext.Oders.Remove(sourceOrder);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        //thêm oder check bàn nếu bàn đã được sử dụng thì false
        public async Task<bool> AddOrderAsync(CreateOderDTO createOrderDTO)
        {
            var table = await _dbContext.Tables.FindAsync(createOrderDTO.TableID);

            if (table == null)
            {
                throw new InvalidOperationException("Table not found");
            }

            if (table.Status != TableStatusConstants.Available)
            {
                // Nếu bàn đã được đặt, từ chối yêu cầu đặt hàng
                return false;
            }

            // Cập nhật trạng thái của bàn
            table.Status = TableStatusConstants.Booked;
            await _dbContext.SaveChangesAsync();
            // Tạo đối tượng Order mới
            var order = new Oders
            {
                AccountID = createOrderDTO.AccountID,
                UserId = createOrderDTO.UserId,
                TableID = createOrderDTO.TableID,
                TotalMoney = createOrderDTO.TotalMoney,
                Status = createOrderDTO.Status,
                CreateDate = DateTime.UtcNow,
                EndDate = createOrderDTO.EndDate
            };

            await _dbContext.Oders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // thay đổi trạng thái khi thanh toán
        public async Task<bool> UpdateOrderStatusOnPayment(int orderId)
        {
            try
            {
                var order = await _dbContext.Oders.FindAsync(orderId);

                if (order == null)
                {
                    throw new InvalidOperationException($"Order with ID {orderId} not found.");
                }

                // Cập nhật trạng thái của đơn đặt hàng thành "Closed" sau khi thanh toán
                order.Status = 4; // Hoặc bất kỳ giá trị trạng thái nào khác tùy theo yêu cầu của bạn

                await _dbContext.SaveChangesAsync();
                var invoice = await _dbContext.HoaDons.FirstOrDefaultAsync(h => h.OderID == orderId);
                if (invoice != null)
                {
                    invoice.Status = "Paid";
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order status: {ex.Message}");
                throw;
            }
        }
    }
}
