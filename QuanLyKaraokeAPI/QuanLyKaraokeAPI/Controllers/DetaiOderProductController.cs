using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.DetailOderProduct;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetaiOderProductController : ControllerBase
    {
        public readonly IDetaiOderPService _detaiOderPService;
        public readonly ILogger<DetaiOderProductController> _logger;
        public DetaiOderProductController(IDetaiOderPService detaiOderPService, ILogger<DetaiOderProductController> logger)
        {
            _detaiOderPService = detaiOderPService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<DetailOderProductDTO>>> GetD()
        {
            try
            {
                var data = await _detaiOderPService.GetCa();
                return new BaseResponse<List<DetailOderProductDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<DetailOderProductDTO>>
                {
                    StatusCode = 200,
                    Message = "Faild"
                };
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateD([FromBody] CreateDetailOderProductDTO createDetailOderProductDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _detaiOderPService.CreateDetailOderP(createDetailOderProductDTO);
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
                await _detaiOderPService.DeleteDetailOderP(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateD(int id, UpdateDetailOderProductDTO updateDetailOderProductDTO)
        {
            try
            {
                await _detaiOderPService.UpdateDetailOderP(id, updateDetailOderProductDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<DetailOderProductDTO> GetById([FromRoute] int id)
        {
            return await _detaiOderPService.GetCaById(id);
        }
    }
}
