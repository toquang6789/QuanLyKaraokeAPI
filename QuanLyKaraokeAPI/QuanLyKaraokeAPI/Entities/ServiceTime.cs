using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class ServiceTime
    {
        [Key]
        public int ServiceID { get; set; }
        public int CategoriesID { get; set; }
        [ForeignKey("CategoriesID")]
        [Required]
        public Categories Categories { get; set; }

        [Required]
        [MaxLength(200)]
        public string ServiceName { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public float OpenPrice { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Status { get; set; }

        public ICollection<DetailOderService> DetailOrdersService { get; set; }
    }
}
