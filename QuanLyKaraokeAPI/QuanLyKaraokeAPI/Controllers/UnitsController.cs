using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.Units;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        public readonly IUnitService _uService;
        public readonly ILogger<UnitsController> _logger;
        public UnitsController(IUnitService uService, ILogger<UnitsController> logger)
        {
            _uService = uService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<UnitsDTO>>> GetU()
        {
            try
            {
                var data = await _uService.GetU();
                return new BaseResponse<List<UnitsDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<UnitsDTO>> { StatusCode = 200, Message = "Faild" };
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateU([FromBody] CreateUnitsDTO createUnitsDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _uService.CreateU(createUnitsDTO);
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
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteU(int id)
        {
            try
            {
                await _uService.DeleteU(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateU(int id, UpdateUnitsDTO updateUnitsDTO)
        {
            try
            {
                await _uService.UpdateU(id, updateUnitsDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<UnitsDTO> GetById([FromRoute] int id)
        {
            return await _uService.GetUById(id);
        }
    }
}
