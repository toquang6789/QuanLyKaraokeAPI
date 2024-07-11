using System.ComponentModel.DataAnnotations;

namespace QuanLyKaraokeAPI.Entities
{
    public class Table
    {
        [Key]
        public int TableID { get; set; }
        [Required]
        [MaxLength(200)]
        public string TabelName { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Status { get; set; }

        public ICollection<Oders> oders { get; set; }
    }
}
