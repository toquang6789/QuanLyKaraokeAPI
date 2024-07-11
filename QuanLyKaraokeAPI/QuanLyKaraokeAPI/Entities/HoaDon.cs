using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class HoaDon
    {
        [Key]
        public int hoaDonID { get; set; }
        [Required]
        public int OderID { get; set; }
        [ForeignKey("OderID")]
        [Required]
        public Oders Oders { get; set; }
        // public string accountName { get; set; }
        [Required]
        public float TotalAmount { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }
}
