using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }

        public int CategoriesID { get; set; }
        [ForeignKey("CategoriesID")]
        [Required]
        public Categories Categories { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Unit { get; set; }
        [Required]
        public float CostPrite { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Status { get; set; }
        public ICollection<DetailOderProduct> detaioderProducts { get; set; }
        public ICollection<DetailImports> detailImports { get; set; }

    }
}
