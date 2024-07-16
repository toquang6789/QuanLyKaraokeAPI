using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.ImportProducts;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportProductServiceController : ControllerBase
    {
        public readonly IIPService _ipService;
        public readonly ILogger<ImportProductServiceController> _logger;
        public ImportProductServiceController(IIPService ipService, ILogger<ImportProductServiceController> logger)
        {
            _ipService = ipService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<ImportProductDTO>>> GetP()
        {
            try
            {
                var data = await _ipService.GetIP();
                return new BaseResponse<List<ImportProductDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<ImportProductDTO>> { StatusCode = 200, Message = "Faild" };
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateO([FromBody] CreateImportProductDTO createImportProductDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _ipService.CreateIP(createImportProductDTO);
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
        public async Task<IActionResult> DeleteP(int id)
        {
            try
            {
                await _ipService.DeleteIP(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateD(int id, UpdateImportProductDTO updateImportProductDTO)
        {
            try
            {
                await _ipService.UpdateIP(id, updateImportProductDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ImportProductDTO> GetById([FromRoute] int id)
        {
            return await _ipService.GetIPById(id);
        }
    }
}
