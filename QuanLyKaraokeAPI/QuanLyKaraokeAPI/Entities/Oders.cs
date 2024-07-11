using QuanLyKaraokeAPI.Entities.Login;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class Oders
    {
        [Key]
        public int OderID { get; set; }
        public int AccountID { get; set; }
        [ForeignKey("AccountID")]
        [Required]
        public Account Account { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public User User { get; set; }

        public int TableID { get; set; }
        [ForeignKey("TableID")]
        [Required]
        public Table Table { get; set; }
        [Required]
        public float TotalMoney { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<DetailOderProduct> detaioderProducts { get; set; }
        public ICollection<DetailOderService> DetailOrdersService { get; set; }
        public ICollection<HoaDon> hoaDons { get; set; }

    }
}
