using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities.Login
{
    public class Account : IdentityUser<int>
    {
        [Key]
        public int AccountID { get; set; }
        [Required]
        [StringLength(200)]
        public string AccountName { get; set; }
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public User User { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int Status { get; set; }

        public ICollection<Oders> oders { get; set; }

    }
}
