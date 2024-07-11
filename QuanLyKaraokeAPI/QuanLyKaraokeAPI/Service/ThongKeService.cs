using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities;

namespace QuanLyKaraokeAPI.Service
{
    public interface ITKService
    {
        //CalculateTotalIncome
        Task<float> TongDoanhThuDH(DateTime startDate, DateTime endDate);
        //CalculateTotalCost
        Task<float> TongChiPhi(DateTime startDate, DateTime endDate);
        //CalculateProfit
        Task<float> Loinhuan(DateTime startDate, DateTime endDate);



    }
    public class ThongKeService : ITKService
    {
        public readonly AppDBContext _dbContext;
        public ThongKeService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<float> Loinhuan(DateTime startDate, DateTime endDate)
        {
            try
            {
                var totalIncome = await TongDoanhThuDH(startDate, endDate);
                var totalCost = await TongChiPhi(startDate, endDate);

                var profit = totalIncome - totalCost;

                return profit;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while calculating profit: {ex.Message}", ex);
            }
        }

        public async Task<float> TongChiPhi(DateTime startDate, DateTime endDate)
        {
            try
            {
                var totalCost = await _dbContext.ImportProducts
                .Where(d => d.CreateDate >= startDate && d.CreateDate <= endDate)
                .SumAsync(d => d.TotalMoney);

                return totalCost;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while calculating total cost: {ex.Message}", ex);
            }
        }

        public async Task<float> TongDoanhThuDH(DateTime startDate, DateTime endDate)
        {
            try
            {
                var totalIncome = await _dbContext.Oders
                    .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate && o.Status == 4) // Lọc các đơn hàng đã đóng
                    .SumAsync(o => o.TotalMoney);

                return totalIncome;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while calculating total income: {ex.Message}", ex);
            }
        }
    }
}
