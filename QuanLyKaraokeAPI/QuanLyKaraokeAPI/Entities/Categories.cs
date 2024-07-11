using System.ComponentModel.DataAnnotations;

namespace QuanLyKaraokeAPI.Entities
{
    public class Categories
    {
        [Key]
        public int CategoriesID { get; set; }
        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; }
        [Required]
        public string Image { get; set; }
        public ICollection<Products> Products { get; set; }
        public ICollection<ServiceTime> ServiceTimes { get; set; }
    }
}
