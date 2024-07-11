using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class DetailImports
    {
        [Key]
        public int Id { get; set; }
        public int IdImport { get; set; }
        [ForeignKey("IdImport")]
        [Required]
        public ImportProducts ImportProducts { get; set; }

        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [Required]
        public Products Products { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int UnitID { get; set; }
        [ForeignKey("UnitID")]
        [Required]
        public Units Units { get; set; }

        [Required]
        public float Monney { get; set; }
    }
}
