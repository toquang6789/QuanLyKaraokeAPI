namespace QuanLyKaraokeAPI.ModelDTO.Account
{
    public class CreateAdminDTO
    {
        //public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int UserId { get; set; }
        //public string UserName { get; set; }
        public int Status { get; set; }
    }
}
