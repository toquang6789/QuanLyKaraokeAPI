using Microsoft.AspNetCore.Identity;

namespace QuanLyKaraokeAPI.Entities.Login
{
    public class Role : IdentityRole<int>
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
