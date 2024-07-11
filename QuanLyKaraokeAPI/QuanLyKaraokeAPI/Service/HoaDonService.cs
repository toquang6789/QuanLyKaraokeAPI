using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using QuanLyKaraokeAPI.Entities;


namespace QuanLyKaraokeAPI.Service
{
    public interface IHDService
    {
        Task<HoaDon> CalculateInvoice(int orderId);
        Task<byte[]> GenerateInvoicePdf(int invoiceId);

    }
    public class HoaDonService : IHDService
    {
        private readonly AppDBContext _context;

        public HoaDonService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<bool> OrderExists(int orderId)
        {
            return await _context.Oders.AnyAsync(o => o.OderID == orderId);
        }
        public async Task<HoaDon> CalculateInvoice(int orderId)
        {
            try
            {
                var orderExists = await OrderExists(orderId);
                if (!orderExists)
                {
                    throw new InvalidOperationException($"Order with ID {orderId} not found.");
                }
                var order = await _context.Oders
               .Include(o => o.detaioderProducts)
                   .ThenInclude(d => d.products)
               .Include(o => o.DetailOrdersService)
                   .ThenInclude(d => d.serviceTime)
               .FirstOrDefaultAsync(o => o.OderID == orderId);

                if (order == null)
                    throw new InvalidOperationException($"Order with ID {orderId} not found.");

                float totalAmountProducts = order.detaioderProducts.Sum(d => d.Quantity * d.products.Price);
                //them
                //float totalAmountServices = order.DetailOrdersService.Sum(d => d.Quantity * d.serviceTime.Price);

                float totalAmountServices = 0;
                foreach (var detail in order.DetailOrdersService)
                {
                    var duration = (detail.EndTime - detail.StartTime).TotalHours;
                    if (duration <= 0)
                    {
                        throw new InvalidOperationException("Thời gian sử dụng dịch vụ không hợp lệ.");
                    }
                    var serviceAmount = (float)(duration * detail.serviceTime.Price + detail.serviceTime.OpenPrice);
                    totalAmountServices += serviceAmount;
                }
                // Tổng tiền của hoá đơn
                float totalAmount = totalAmountProducts + totalAmountServices;

                var invoice = new HoaDon
                {
                    OderID = order.OderID,
                    //accountName = order.Account.AccountName,
                    TotalAmount = totalAmount,
                    Status = "Pending", // Example status
                    CreatedAt = DateTime.Now,
                    EndDate = DateTime.Now // Example end date
                };

                _context.HoaDons.Add(invoice);
                await _context.SaveChangesAsync();

                return invoice;
            }
            catch (DbUpdateException dbEx)
            {
                // Ghi nhật ký lỗi chi tiết từ lỗi bên trong
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                throw new Exception($"An error occurred while saving the invoice. See the inner exception for details: {innerExceptionMessage}", dbEx);
            }
            catch (Exception ex)
            {
                // Ghi nhật ký lỗi chi tiết từ lỗi tổng quát
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while calculating the invoice. See the inner exception for details: {innerExceptionMessage}", ex);
            }
        }

        public async Task<byte[]> GenerateInvoicePdf(int invoiceId)
        {
            try
            {
                var invoice = await _context.HoaDons
                .Include(i => i.Oders)
                     .ThenInclude(o => o.detaioderProducts)
                     .ThenInclude(d => d.products)
                .Include(i => i.Oders)
                     .ThenInclude(o => o.DetailOrdersService)
                     .ThenInclude(d => d.serviceTime)
                     .FirstOrDefaultAsync(i => i.hoaDonID == invoiceId);

                if (invoice == null)
                    throw new InvalidOperationException("Invoice not found");

                // Generate PDF content programmatically
                //string pdfContent = $"Invoice for Order ID: {invoice.OderID}\n";
                //pdfContent += $"Total Money: {invoice.TotalAmount}\n";
                //pdfContent += "Order Details:\n";

                //foreach (var item in invoice.Oders.detaioderProducts)
                //{
                //    pdfContent += $"- Product: {item.products.ProductName}, Quantity: {item.Quantity}, Unit Price: {item.products.Price}, Total Price: {item.Quantity * item.products.Price}\n";
                //}

                //return Encoding.UTF8.GetBytes(pdfContent);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (PdfDocument pdf = new PdfDocument())
                    {
                        PdfPage page = pdf.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        XFont font = new XFont("Arial", 12, XFontStyle.Regular);

                        // Draw content on PDF page
                        gfx.DrawString($"Invoice for Order ID: {invoice.OderID}", font, XBrushes.Black, new XRect(10, 10, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString($"Total Money: {invoice.TotalAmount}", font, XBrushes.Black, new XRect(10, 30, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString($"Created At: {invoice.CreatedAt}", font, XBrushes.Black, new XRect(10, 70, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString($"End Date: {invoice.EndDate}", font, XBrushes.Black, new XRect(10, 90, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Order Details:", font, XBrushes.Black, new XRect(10, 50, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                        int yPos = 140;
                        foreach (var item in invoice.Oders.detaioderProducts)
                        {
                            string line = $"- Product: {item.products.ProductName}, Quantity: {item.Quantity}, Unit Price: {item.products.Price}, Total Price: {item.Quantity * item.products.Price}";
                            gfx.DrawString(line, font, XBrushes.Black, new XRect(10, yPos, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            yPos += 20;
                        }
                        gfx.DrawString("Service Details:", font, XBrushes.Black, new XRect(10, yPos + 20, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                        yPos += 40;
                        foreach (var serviceDetail in invoice.Oders.DetailOrdersService)
                        {
                            string serviceLine = $"- Service: {serviceDetail.serviceTime.ServiceName}, Quantity: {serviceDetail.Quantity}, Start Time: {serviceDetail.StartTime}, End Time: {serviceDetail.EndTime}";
                            gfx.DrawString(serviceLine, font, XBrushes.Black, new XRect(10, yPos, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            yPos += 20;
                        }

                        pdf.Save(ms);
                    }

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
                throw;
            }
        }


    }
}
