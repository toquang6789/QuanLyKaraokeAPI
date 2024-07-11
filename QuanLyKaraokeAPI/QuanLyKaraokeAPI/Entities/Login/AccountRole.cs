using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKaraokeAPI.Entities.Login
{
    public class AccountRole : IdentityUserRole<int>
    {
        public int AccountID { get; set; }
        [ForeignKey("AccountID")]
        public Account Account { get; set; }

        //public int UserId { get; set; }
        //[ForeignKey("UserId")]
        //public User User { get; set; }


        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public Role Role { get; set; }
    }
}
