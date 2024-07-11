using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.ModelDTO;
using QuanLyKaraokeAPI.ModelDTO.Oder;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OderController : ControllerBase
    {
        public readonly IOService _oService;
        public readonly ILogger<OderController> _logger;
        public OderController(IOService oService, ILogger<OderController> logger)
        {
            _oService = oService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<BaseResponse<List<OderDTO>>> GetP()
        {
            try
            {
                var data = await _oService.GetO();
                return new BaseResponse<List<OderDTO>> { StatusCode = 200, Message = "Sucessed", Data = data };
            }
            catch (Exception)
            {
                return new BaseResponse<List<OderDTO>> { StatusCode = 200, Message = "Faild" };
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateO([FromBody] CreateOderDTO createOderDTO)
        {
            if (!ModelState.IsValid) return Problem("Validate failed");
            try
            {
                await _oService.CreateO(createOderDTO);
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
                await _oService.DeleteO(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID");
            }
            return Ok("Delete Succesfully");
        }
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateD(int id, UpdateOderDTO updateOderDTO)
        {
            try
            {
                await _oService.UpdateO(id, updateOderDTO);
                return Ok("Update Succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<OderDTO> GetById([FromRoute] int id)
        {
            return await _oService.GetOById(id);
        }


        [HttpPost("merge")]
        public async Task<IActionResult> MergeOrders([FromBody] MergeOrdersRequest request)
        {
            var result = await _oService.MergeOrdersAsync(request.SourceOrderId, request.TargetOrderId);
            if (!result) return BadRequest();
            return Ok();
        }

        public class MergeOrdersRequest
        {
            public int SourceOrderId { get; set; }
            public int TargetOrderId { get; set; }
        }


        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder(CreateOderDTO createOrderDTO)
        {
            try
            {
                var isOrderAdded = await _oService.AddOrderAsync(createOrderDTO);

                if (!isOrderAdded)
                {
                    return BadRequest("Table is not available for booking.");
                }

                return Ok("Order added and table reserved successfully.");
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


        [HttpPost("thanhtoanstatus-order/{orderId}")]
        public async Task<IActionResult> PayOrder(int orderId)
        {
            try
            {
                var result = await _oService.UpdateOrderStatusOnPayment(orderId);

                if (!result)
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }

                return Ok($"Order with ID {orderId} has been successfully paid and closed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing payment: {ex.Message}");
            }
        }
    }
}
