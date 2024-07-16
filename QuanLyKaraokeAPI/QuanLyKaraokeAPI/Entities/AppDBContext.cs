using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyKaraokeAPI.Entities.Login;
using QuanLyKaraokeAPI.ModelDTO.Account;

namespace QuanLyKaraokeAPI.Entities
{
    public class AppDBContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        //public AppDBContext(DbContextOptions<AppDBContext> options): base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<DetailImports> DetailImports { get; set; }
        public DbSet<DetailOderProduct> OrdersProduct { get; set; }
        public DbSet<DetailOderService> OrdersService { get; set; }
        public DbSet<ImportProducts> ImportProducts { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ServiceTime> ServiceTime { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Oders> Oders { get; set; }
        public DbSet<Units> units { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Oders>()
                .HasOne(o => o.Account)
                .WithMany(a => a.oders)
                .HasForeignKey(o => o.AccountID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Oders>()
                .HasOne(o => o.User)
                .WithMany(u => u.oders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Oders>()
                .HasOne(o => o.Table)
                .WithMany(t => t.oders)
                .HasForeignKey(o => o.TableID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

            // Configure the Account entity
            modelBuilder.Entity<Account>().ToTable("Account")
                .HasKey(a => a.AccountID);

            modelBuilder.Entity<User>()
           .HasMany(u => u.oders)
           .WithOne(o => o.User)
           .HasForeignKey(o => o.UserId)
           .OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Account>()
            //.HasOne(a => a.User)
            //.WithMany()
            // .HasForeignKey(a => a.UserId)
            // .IsRequired();


            //modelBuilder.Entity<Account>()
            //    .HasMany(a => a.oders)
            //    .WithOne(o => o.Account)
            //    .HasForeignKey(o => o.AccountID)
            //    .OnDelete(DeleteBehavior.Cascade);


            // Configure the Role entity
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleID);


            // Configure the other Identity entities

            modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });

            // Define primary key for IdentityRoleClaim
            modelBuilder.Entity<IdentityRoleClaim<int>>().HasKey(rc => rc.Id);

            // Define primary key for IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<int>>().HasKey(ur => new { ur.UserId, ur.RoleId });

            // Define primary key for IdentityUserToken
            modelBuilder.Entity<IdentityUserToken<int>>().HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });


        }


    }

}
