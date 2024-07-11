namespace QuanLyKaraokeAPI.ModelDTO.User
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string fullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime Birthday { get; set; }
        public bool Sex { get; set; }

        public string Avatar { get; set; }

        public bool TypeUser { get; set; }

        public int Status { get; set; }
    }
}
