using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuanLyKaraokeAPI.Entities
{
    public class DetailOderProduct
    {
        [Key]
        public int Id { get; set; }
        public int OderID { get; set; }
        [ForeignKey("OderID")]
        [Required]
        [JsonIgnore]
        public Oders oders { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [Required]
        public Products products { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime TimeOder { get; set; }
        [Required]
        public int Status { get; set; }

    }
}
