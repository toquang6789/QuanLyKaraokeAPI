using iText.Kernel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using QuanLyKaraokeAPI.Service;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHDService _invoiceService;
        public HoaDonController(IHDService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // POST: api/invoices/calculate
        [HttpPost("Calculate")]
        public async Task<IActionResult> CalculateInvoice(int orderId)
        {

            try
            {
                var invoice = await _invoiceService.CalculateInvoice(orderId);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/invoices/print/{invoiceId}
        [HttpGet("Print/{invoiceId}")]
        public async Task<IActionResult> PrintInvoice(int invoiceId)
        {
            try
            {
                byte[] pdfBytes = await _invoiceService.GenerateInvoicePdf(invoiceId);
                return File(pdfBytes, "application/pdf", $"Invoice_{invoiceId}.pdf");
            }
            catch (PdfException ex)
            {
                // Ghi log lỗi PDFException
                Console.WriteLine($"PDF generation error: {ex.Message}");
                Console.WriteLine($"PDF generation stack trace: {ex.StackTrace}");
                return BadRequest($"Error generating PDF: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Ghi log lỗi InvalidOperationException
                Console.WriteLine($"Invalid operation: {ex.Message}");
                Console.WriteLine($"Invalid operation stack trace: {ex.StackTrace}");
                return BadRequest($"Invalid operation: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Ghi log các ngoại lệ khác
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.WriteLine($"Unexpected error stack trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: {ex.Message}");
            }
        }
    }
}
