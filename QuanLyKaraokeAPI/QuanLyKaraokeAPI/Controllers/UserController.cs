using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.User;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUService _usService;
        public readonly ILogger<UserController> _logger;

        public UserController(IUService usService, ILogger<UserController> logger)
        {
            _usService = usService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<BaseResponse<List<UserDTO>>> GetAll()
        {
            // List<UserDTO> data;
            try
            {
                var data = await _usService.GetUser();
                return new BaseResponse<List<UserDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<UserDTO>> { StatusCode = 200, Message = "Faild" };
            }

        }
        [HttpGet("{id}")]
        public async Task<UserDTO> GetById([FromRoute] int id)
        {
            return await _usService.GetUserById(id);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateUserDTO model)
        {

            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _usService.CreateUser(model);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usService.DeleteUser(id);
            }
            catch (Exception x)
            {
                _logger.LogError(x, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDTO model)
        {
            try
            {
                await _usService.UpdateUser(id, model);
                return Ok("Update succefully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}
