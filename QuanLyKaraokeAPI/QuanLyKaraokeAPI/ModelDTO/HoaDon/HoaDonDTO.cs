namespace QuanLyKaraokeAPI.ModelDTO.HoaDon
{
    public class HoaDonDTO
    {
        public string hoaDonID { get; set; }
        public int OrderId { get; set; }
        public string Order { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
