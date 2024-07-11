using QuanLyKaraokeAPI.Entities.Login;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKaraokeAPI.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(200)]
        public string fullName { get; set; }
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(200)]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public bool Sex { get; set; }
        [Required]
        public string Avatar { get; set; }
        [Required]
        public bool TypeUser { get; set; }
        [Required]
        public int Status { get; set; }
        public ICollection<Oders> oders { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<ImportProducts> ImportProducts { get; set; }

    }
}
