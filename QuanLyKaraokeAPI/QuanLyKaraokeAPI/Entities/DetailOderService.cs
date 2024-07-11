using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class DetailOderService
    {
        [Key]
        public int Id { get; set; }
        public int OderID { get; set; }
        [ForeignKey("OderID")]
        [Required]
        public Oders oders { get; set; }

        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        [Required]
        public ServiceTime serviceTime { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
