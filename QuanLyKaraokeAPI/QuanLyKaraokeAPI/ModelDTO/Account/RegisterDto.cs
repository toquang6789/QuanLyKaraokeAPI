using System.ComponentModel.DataAnnotations;

namespace QuanLyKaraokeAPI.ModelDTO.Account
{
    public class RegisterDto
    {
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public bool Sex { get; set; }
        [Required]
        public IFormFile Avatar { get; set; }
        //public int Status { get; set; }
        public bool TypeUser { get; set; }
    }
}
