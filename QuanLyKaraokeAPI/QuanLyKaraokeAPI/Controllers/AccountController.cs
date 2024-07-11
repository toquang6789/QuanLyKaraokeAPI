using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.Entities.Login;
using QuanLyKaraokeAPI.ModelDTO.Account;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace QuanLyKaraokeAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDBContext context, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        //{
        //    var existingUser = await _userManager.FindByNameAsync(registerDto.Email);
        //    if (existingUser != null)
        //    {
        //        return BadRequest("Username is already taken.");
        //    }
        //    string uniqueFileName = UploadedFile(registerDto);
        //    var user = new User
        //    {
        //        fullName = registerDto.FullName,
        //        Email = registerDto.Email,
        //        Phone = registerDto.Phone,
        //        Address = registerDto.Address,
        //        Birthday = registerDto.Birthday,
        //        Sex = registerDto.Sex,
        //        Avatar = uniqueFileName,
        //        Status = 1,
        //        TypeUser = registerDto.TypeUser ? true : false
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    var account = new Account
        //    {
        //        UserName = registerDto.Email,
        //        Email = registerDto.Email,
        //        Password = registerDto.Password,
        //        AccountName = registerDto.AccountName,
        //        UserId = user.UserId,
        //        Status = user.Status
        //    };

        //    var result = await _userManager.CreateAsync(account, registerDto.Password);

        //    if (result.Succeeded)
        //    {

        //        var roleExists = await _roleManager.RoleExistsAsync("User");
        //        if (!roleExists)
        //        {
        //            var role = new Role
        //            {
        //                Name = "User",  // Tên vai trò trong IdentityRole
        //                RoleName = "User"  // Tên vai trò trong lớp Role của bạn
        //            };
        //            var roleResult = await _roleManager.CreateAsync(role);
        //            if (!roleResult.Succeeded)
        //            {
        //                await _userManager.DeleteAsync(account);
        //                return BadRequest("Failed to create role");
        //            }
        //        }

        //        var addToRoleResult = await _userManager.AddToRoleAsync(account, "User");
        //        if (addToRoleResult.Succeeded)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            // Nếu không thêm được vào vai trò, xóa tài khoản và vai trò đã tạo và trả về lỗi
        //            await _userManager.DeleteAsync(account);
        //            await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync("User"));
        //            return BadRequest("Failed to add user to role 'User'.");
        //        }
        //    }

        //    else
        //    {
        //        // Nếu không tạo được tài khoản, xóa người dùng vừa tạo và trả về lỗi
        //        _context.Users.Remove(user);
        //        await _context.SaveChangesAsync();
        //        return BadRequest(result.Errors);
        //    }
        //}

        [HttpPost("dangki")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DangKi([FromForm] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return NotFound("User not found");
            }
            string uniqueFileName = UploadedFile(registerDto);
            var user = new User
            {
                fullName = registerDto.FullName,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                Address = registerDto.Address,
                Birthday = registerDto.Birthday,
                Sex = registerDto.Sex,
                Avatar = uniqueFileName,
                Status = 1,
                TypeUser = registerDto.TypeUser ? true : false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //var account = new Account
            //{
            //    //UserName = registerDto.AccountName,
            //    Email = registerDto.Email,
            //    Password = registerDto.Password,
            //    AccountName = registerDto.AccountName,
            //    UserId = user.UserId,
            //    Status = user.Status
            //};
            //_context.Accounts.Add(account);
            //await _context.SaveChangesAsync();


            var newuser = new IdentityUser
            {
                UserName = registerDto.FullName,
                Email = registerDto.Email,
            };
            var useremail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (useremail != null)
            {
                return BadRequest("User registered already");
            }
            var registerUser = await _userManager.CreateAsync(newuser, registerDto.Password);
            if (!registerUser.Succeeded) return BadRequest("Them nguoi dung khong thanh cong");
            string roleName = registerDto.TypeUser ? "Nhanvien" : "Khachhang";
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    return BadRequest("Tạo vai trò không thành công");
                }
            }
            var addToRoleResult = await _userManager.AddToRoleAsync(newuser, roleName);
            //var role = await _roleManager.FindByNameAsync("Admin");
            //if (role == null)
            //{
            //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
            //}
            //var addToRoleResult = await _userManager.AddToRoleAsync(newuser, "Admin");
            if (!addToRoleResult.Succeeded) return BadRequest("Them nguoi dung vao vai tro khong thanh cong");
            return Ok("Tao tai khoan Thanh cong");

        }


        private string ConvertToBase64(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageArray);
        }
        private string UploadedFile(RegisterDto model)
        {
            string uniqueFileName = null;
            if (model.Avatar == null)
                return null;
            string webRootPath = _webHostEnvironment.ContentRootPath ?? throw new ArgumentNullException(nameof(_webHostEnvironment.WebRootPath), "Web root path cannot be null.");


            string uploadsFolder = Path.Combine(webRootPath, "Image");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Avatar.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Avatar.CopyTo(fileStream);
            }


            return ConvertToBase64(filePath);
        }






        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrorMessageFromModel(ModelState));
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                return Ok(token);
            }
            return NotFound("User not found");
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var identityClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],
                Subject = new ClaimsIdentity(identityClaims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Duration"])),
                SigningCredentials = signingCredentials,
                Claims = new Dictionary<string, object> { { "UserId", user.Id } }
            };
            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return token;
        }

        private string GetErrorMessageFromModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(s => s.Errors).ToList();
            var messages = errors.Select(s => s.ErrorMessage);
            var result = string.Join(", ", messages);
            return result;
        }


        [HttpPost("CreateAdmin")]
        [Authorize(Roles = "Admin")] // Yêu cầu quyền Admin để truy cập
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO adminDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(adminDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already taken.");
            }
            var account = new Account
            {
                AccountName = adminDto.Name,
                Email = adminDto.Email,
                Password = adminDto.Password,
                UserId = adminDto.UserId,
                Status = adminDto.Status,
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            var newUser = new IdentityUser
            {
                UserName = adminDto.Email,
                Email = adminDto.Email,
            };

            var result = await _userManager.CreateAsync(newUser, adminDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to create admin account.");
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "Admin");
            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser); // Rollback: Xóa tài khoản nếu không thêm được vào vai trò
                return BadRequest("Failed to assign admin role.");
            }

            return Ok("Admin account created successfully.");
        }

        // Controller action
        [Authorize(Roles = "Admin")]
        [HttpPost("updateTypeUser")]
        public async Task<IActionResult> UpdateTypeUser([FromBody] UpdateTypeDTO updateTypeUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == updateTypeUserDto.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update TypeUser based on the value from DTO
            user.TypeUser = updateTypeUserDto.TypeUser;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(updateTypeUserDto.UserId))
                {
                    return NotFound("User not found");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Update TypeUser successful");
        }

        private bool UserExists(int userId)
        {
            return _context.Users.Any(e => e.UserId == userId);
        }
    }
}
