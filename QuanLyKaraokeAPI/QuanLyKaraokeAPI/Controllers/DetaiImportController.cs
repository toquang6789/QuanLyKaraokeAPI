using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.DetailImports;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetaiImportController : ControllerBase
    {
        public readonly IDetaiImportService _ipService;
        public readonly ILogger<OderController> _logger;
        public DetaiImportController(IDetaiImportService ipService, ILogger<OderController> logger)
        {
            _ipService = ipService;
            _logger = logger;
        }
        [HttpGet]
        [Authorize(Roles = "Khachhang,Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<BaseResponse<List<DetaiImportsDTO>>> GetP()
        {
            try
            {
                var data = await _ipService.GetDI();
                return new BaseResponse<List<DetaiImportsDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<DetaiImportsDTO>> { StatusCode = 500, Message = "Failed" };

            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateO([FromBody] CreateDetailImportDTO createDetailImportDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _ipService.CreateDetailI(createDetailImportDTO);
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
        public async Task<IActionResult> DeleteP(int id)
        {
            try
            {
                await _ipService.DeleteDetailI(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateD(int id, UpdateDetailImportDTO updateDetailImportDTO)
        {
            try
            {
                await _ipService.UpdateDetailI(id, updateDetailImportDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<DetaiImportsDTO> GetById([FromRoute] int id)
        {
            return await _ipService.GetDIById(id);
        }
    }
}
