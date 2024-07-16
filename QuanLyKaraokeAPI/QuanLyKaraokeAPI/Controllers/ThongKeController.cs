using Microsoft.AspNetCore.Mvc;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        public readonly ITKService _tkService;
        public readonly ILogger<TableController> _logger;
        public ThongKeController(ITKService tkService, ILogger<TableController> logger)
        {
            _tkService = tkService;
            _logger = logger;
        }
        //tong doanh thu
        [HttpGet("Total-Income")]
        public async Task<IActionResult> GetTotalIncome(DateTime startDate, DateTime endDate)
        {
            try
            {
                var totalIncome = await _tkService.TongDoanhThuDH(startDate, endDate);
                return Ok(totalIncome);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving total income: {ex.Message}");
            }
        }
        //tong chi phi
        [HttpGet("Total-Cost")]
        public async Task<IActionResult> GetTotalCost(DateTime startDate, DateTime endDate)
        {
            try
            {
                var totalCost = await _tkService.TongChiPhi(startDate, endDate);
                return Ok(totalCost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving total cost: {ex.Message}");
            }
        }

        //loi nhuan
        [HttpGet("Profit")]
        public async Task<IActionResult> GetProfit(DateTime startDate, DateTime endDate)
        {
            try
            {
                var profit = await _tkService.Loinhuan(startDate, endDate);
                return Ok(profit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while calculating profit: {ex.Message}");
            }
        }
    }
}
