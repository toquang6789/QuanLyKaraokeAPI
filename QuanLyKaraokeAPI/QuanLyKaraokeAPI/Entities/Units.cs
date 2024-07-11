using System.ComponentModel.DataAnnotations;

namespace QuanLyKaraokeAPI.Entities
{
    public class Units
    {
        [Key]
        public int UnitID { get; set; }
        [Required]
        [MaxLength(250)]
        public string UnitName { get; set; }
        [Required]
        public int ConvertUnit { get; set; }

        public ICollection<DetailImports> detailImports { get; set; }
    }
}
