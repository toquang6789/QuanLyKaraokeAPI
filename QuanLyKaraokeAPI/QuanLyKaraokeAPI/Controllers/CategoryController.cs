using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.Categories;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICService _cService;
        public readonly ILogger<TableController> _logger;
        public CategoryController(ICService cService, ILogger<TableController> logger)
        {
            _cService = cService;
            _logger = logger;
        }
        [HttpGet]

        [Authorize(Roles = "Nhanvien,Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<BaseResponse<List<CategoryDTO>>> GetTable()
        {
            try
            {
                var data = await _cService.GetCa();
                return new BaseResponse<List<CategoryDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<CategoryDTO>> { StatusCode = 500, Message = "Failed" };
            }
        }

        [HttpGet("{id}")]
        public async Task<CategoryDTO?> GetTableById(int id)
        {
            try
            {
                return await _cService.GetCaById(id);

            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateTable([FromForm] CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                await _cService.CreateCa(createCategoryDTO);
                return Ok("Create Succesfully");
            }
            catch (DbUpdateException dbEx)
            {
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
        public async Task<IActionResult> DeleteTable(int id)
        {
            try
            {
                await _cService.DeleteCa(id);
                return Ok("Delete Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateTable(int id, UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                await _cService.UpdateCa(id, updateCategoryDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

    }
}
