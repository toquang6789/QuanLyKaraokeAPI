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
using QuanLyKaraokeAPI.Service;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger,IEmailSender emailSender,RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }
       

        [HttpPost("DangKi")]
        [AllowAnonymous]
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
                Status = 0,
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


            var newuser = new ApplicationUser
            {
                UserName = registerDto.FullName,
                Email = registerDto.Email,
                Status = 0,
            };


            var useremail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (useremail != null)
            {
                return BadRequest("User registered already");
            }
            var registerUser = await _userManager.CreateAsync(newuser, registerDto.Password);
            if (!registerUser.Succeeded)
            {
               // _logger.LogError("Error registering user: {Errors}", string.Join(", ", registerUser.Errors.Select(e => e.Description)));
                return BadRequest("Them nguoi dung khong thanh cong");
            }
            string roleName = user.TypeUser ? "Nhanvien" : "Khachhang";
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
            if (!addToRoleResult.Succeeded) return BadRequest("Thêm người dùng vào vai trò không thành công");

            try
            {
                // Your registration logic here...

                // Get all users with the Admin role
                var adminRole = await _roleManager.FindByNameAsync("Admin");
                if (adminRole != null)
                {
                    var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                    foreach (var admin in adminUsers)
                    {
                        if (!string.IsNullOrWhiteSpace(admin.Email))
                        {
                            await _emailSender.SendEmailAsync(admin.Email, "Thông báo tài khoản mới đăng ký", $"Tài khoản {registerDto.FullName} đã đăng ký và cần được duyệt.");
                        }
                        else
                        {
                            _logger.LogWarning($"Admin user {admin.UserName} does not have a valid email address.");
                        }
                    }
                }

               // return Ok("Tạo tài khoản thành công. Vui lòng chờ admin kích hoạt tài khoản.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to admins");
                return StatusCode(500, "Lỗi khi gửi email thông báo đến admin.");
            }



            return Ok("Tạo tài khoản thành công. Vui lòng chờ admin kích hoạt tài khoản.");
           

        }


        [HttpPost("KichHoatUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser([FromBody] XacNhanTK activateUserDto)
        {
            var user = await _context.Users.FindAsync(activateUserDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Status = 1; // Activate user
            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                return NotFound("Identity user not found");
            }
            var appUser = _userManager.FindByIdAsync(identityUser.Id);
            identityUser.Status = 1;
            var updateStatus = await _userManager.UpdateAsync(identityUser);
            if (!updateStatus.Succeeded)
            {
                return BadRequest("Cập nhập trạng thái tài khoản không thành công");
            }
            return Ok("Kích hoạt tài khoản thành công");
        }


        [HttpGet("GetDSTaikhoanChuaKichHoat")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPendingActivationUsers()
        {
            var pendingUsers = _context.Users.Where(u => u.Status == 0).ToList();
            return Ok(pendingUsers);
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






        [HttpPost("Login")]
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
         
            if (user.Status != 1)
            {
                return BadRequest("Tài khoản của bạn chưa được kích hoạt.");
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                return Ok(token);
            }
            return NotFound("User not found");
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
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
            var newUser = new ApplicationUser
            {
                UserName = adminDto.Email,
                Email = adminDto.Email,
                Status = 1
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
        [HttpPost("UpdateTypeUser")]
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
            //if (user.Status != 1)
            //{
            //    return BadRequest("Tài khoản của bạn chưa được kích hoạt.");
            //}
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
