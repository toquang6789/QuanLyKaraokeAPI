using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.DetailOderService;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailOderServiceController : ControllerBase
    {
        private readonly IDetaiOderSService _detaiOderSService;
        private readonly ILogger<DetailOderServiceController> _logger;
        public DetailOderServiceController(IDetaiOderSService detaiOderSService, ILogger<DetailOderServiceController> logger)
        {
            _detaiOderSService = detaiOderSService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<DetaiOderServiceDTO>>> GetD()
        {
            try
            {
                var data = await _detaiOderSService.GetS();
                return new BaseResponse<List<DetaiOderServiceDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<DetaiOderServiceDTO>>
                {
                    StatusCode = 200,
                    Message = "Faild",
                };
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateD([FromBody] CreateDetaiOderServiceDTO createDetaiOderServiceDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _detaiOderSService.CreateDetailOderS(createDetaiOderServiceDTO);
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
                await _detaiOderSService.DeleteDetailOderS(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateD(int id, UpdateDetailOderServiceDTO updateDetailOderServiceDTO)
        {
            try
            {
                await _detaiOderSService.UpdateDetailOderS(id, updateDetailOderServiceDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<DetaiOderServiceDTO> GetById([FromRoute] int id)
        {
            return await _detaiOderSService.GetSById(id);
        }
    }
}
