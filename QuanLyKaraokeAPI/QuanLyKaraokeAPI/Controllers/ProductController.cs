using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.Products;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProService _pService;
        public readonly ILogger<ProductController> _logger;
        public ProductController(IProService pService, ILogger<ProductController> logger)
        {
            _pService = pService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<ProductDTO>>> GetP()
        {
            try
            {
                var data = await _pService.GetP();
                return new BaseResponse<List<ProductDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<ProductDTO>> { StatusCode = 200, Message = "Faild" };
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateP([FromForm] CreateProductDTO createProductDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _pService.CreateP(createProductDTO);
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
                await _pService.Deletep(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateP(int id, UpdateProductDTO updateProductDTO)
        {
            try
            {
                await _pService.UpdateP(id, updateProductDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ProductDTO> GetById([FromRoute] int id)
        {
            return await _pService.GetPById(id);
        }
    }
}
