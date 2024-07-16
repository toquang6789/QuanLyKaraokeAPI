using Microsoft.AspNetCore.Mvc;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.Supplier;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        public readonly ISuppService _suppService;
        public readonly ILogger<SupplierController> _logger;
        public SupplierController(ISuppService suppService, ILogger<SupplierController> logger)
        {
            _suppService = suppService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<SupplierDTO>>> GetSupp()
        {
            try
            {
                var data = await _suppService.GetSupp();
                return new BaseResponse<List<SupplierDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<SupplierDTO>> { StatusCode = 200, Message = "Faild" };
            }
        }

        [HttpGet("{id}")]
        public async Task<SupplierDTO> GetByID([FromRoute] int id)
        {
            return await _suppService.GetSuppById(id);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateSupp([FromBody] CreateSupplierDTO createSupplierDTO)
        {
            try
            {
                await _suppService.CreateSupp(createSupplierDTO);
                return Ok("Create Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            try
            {
                await _suppService.DeleteSupp(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateSupp(int id, UpdateSupplierDTO model)
        {
            try
            {
                await _suppService.UpdateSupp(id, model);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}
