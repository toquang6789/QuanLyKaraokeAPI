using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities
{
    public class ImportProducts
    {
        [Key]
        public int IdImport { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int SupplierID { get; set; }
        [ForeignKey("SupplierID")]
        public Supplier Supplier { get; set; }
        [Required]
        public float TotalMoney { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public int Status { get; set; }
        public ICollection<DetailImports> detailImports { get; set; }

    }
}
