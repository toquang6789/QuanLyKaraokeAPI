using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.ServiceTime;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTimeController : ControllerBase
    {
        public readonly ISTimeService _sService;
        public readonly ILogger<ServiceTimeController> _logger;
        public ServiceTimeController(ISTimeService sService, ILogger<ServiceTimeController> logger)
        {
            _sService = sService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<ServiceTimeDTO>>> GetS()
        {
            try
            {
                var data = await _sService.GetSTime();
                return new BaseResponse<List<ServiceTimeDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<ServiceTimeDTO>> { StatusCode = 200, Message = "Faild" };
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateS([FromForm] CreateServiceTimeDTO createServiceTimeDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _sService.CreateSTime(createServiceTimeDTO);
                return Ok("Create Succesfully");
            }
            catch (DbUpdateException dbEx)
            {
                // Lấy thông tin chi tiết từ lỗi bên trong (inner exception)
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Lỗi server nội bộ: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteS(int id)
        {
            try
            {
                await _sService.DeleteSTime(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateP(int id, UpdateServiceTimeDTO updateServiceTimeDTO)
        {
            try
            {
                await _sService.UpdateSTime(id, updateServiceTimeDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ServiceTimeDTO> GetSTimeById([FromRoute] int id)
        {
            return await _sService.GetSTimeById(id);
        }
    }
}
