using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuanLyKaraokeAPI;
using QuanLyKaraokeAPI.Entities;
using QuanLyKaraokeAPI.Service;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectstring = builder.Configuration.GetConnectionString("quanlykaraokeAPI1");
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectstring);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Nhanvien", policy => policy.RequireRole("Nhanvien"));
    options.AddPolicy("Khachhang", policy => policy.RequireRole("Khachhang"));
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUService, UserService>();
builder.Services.AddScoped<ISuppService, SupplierService>();
builder.Services.AddScoped<ITService, TableService>();
builder.Services.AddScoped<ICService, CategoryService>();
builder.Services.AddScoped<IProService, ProductService>();
builder.Services.AddScoped<ISTimeService, ServiceTimeService>();
builder.Services.AddScoped<IOService, OderService>();
builder.Services.AddScoped<IIPService, ImportProductService>();
builder.Services.AddScoped<IDetaiOderPService, DetailOderProductService>();
builder.Services.AddScoped<IDetaiImportService, DetailImportService>();
builder.Services.AddScoped<IUnitService, UnitsService>();
builder.Services.AddScoped<IHDService, HoaDonService>();
builder.Services.AddScoped<IDetaiOderSService, DetailOderSeService>();
builder.Services.AddScoped<ITKService, ThongKeService>();
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.AllowAnyHeader().WithOrigins("*").AllowAnyMethod();
    });
});

var app = builder.Build();




var env = builder.Environment;
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();

// Đảm bảo sử dụng để phục vụ các tệp tĩnh từ thư mục wwwroot

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigin");
app.UseHttpsRedirection();
//var appEnv = app.Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
