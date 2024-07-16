using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.Table;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        public readonly ITService _tService;
        public readonly ILogger<TableController> _logger;
        public TableController(ITService tService, ILogger<TableController> logger)
        {
            _tService = tService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<TableDTO>>> GetTable()
        {
            try
            {
                var data = await _tService.GetTable();
                return new BaseResponse<List<TableDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<TableDTO>> { StatusCode = 200, Message = "Faild" };
            }


        }

        [HttpGet("{id}")]
        public async Task<TableDTO> GetTableById(int tableId)
        {
            return await _tService.GetTableById(tableId);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateTable([FromForm] CreateTableDTO createTableDTO)
        {
            try
            {
                await _tService.CreateTable(createTableDTO);
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
                await _tService.DeleteTable(id);
                return Ok("Delete Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateTable(int id, UpdateTableDTO updateTableDTO)
        {
            try
            {
                await _tService.UpdateTable(id, updateTableDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }


        [HttpPut("Updatestatus/{id}")]
        public async Task<IActionResult> UpdateTableStatus(int id, [FromBody] int status)
        {
            var result = await _tService.UpdateTableStatusAsync(id, status);
            if (!result) return BadRequest();
            return NoContent();
        }

        [HttpPost("Reserve-Table")]
        public async Task<IActionResult> ReserveTable(int tableId)
        {
            try
            {
                var isReserved = await _tService.ReserveTable(tableId);

                if (!isReserved)
                {
                    return BadRequest("Table is not available for booking.");
                }

                return Ok("Table reserved successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

    }
}
