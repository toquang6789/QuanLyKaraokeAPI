using Microsoft.AspNetCore.Identity;

namespace QuanLyKaraokeAPI.ModelDTO.Account
{
    public class ApplicationUser :IdentityUser
    {
        
        public int Status {  get; set; }
    }
}
