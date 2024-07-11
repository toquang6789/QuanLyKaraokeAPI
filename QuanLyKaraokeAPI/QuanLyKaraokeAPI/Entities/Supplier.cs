using System.ComponentModel.DataAnnotations;

namespace QuanLyKaraokeAPI.Entities
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        [Required]
        [MaxLength(100)]
        public string SupplierName { get; set; }
        [Required]
        [MaxLength(100)]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public ICollection<ImportProducts> ImportProducts { get; set; }
    }
}
