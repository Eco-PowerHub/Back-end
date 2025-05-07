using EcoPowerHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Data
{
    public class EcoPowerDbContext : IdentityDbContext<ApplicationUser>
    {
        //  public EcoPowerDbContext() { }
        public EcoPowerDbContext(DbContextOptions<EcoPowerDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPackage> ProductPackages { get; set; }
        public DbSet<Package> Packages { get; set; }

        public DbSet<UserProperty> UserProperties { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOreders { get; set; }
        public DbSet<UserFeedBack> UserFeedBacks { get; set; }
        public DbSet<UserSupport> UserSupport { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
            //    if (!optionsBuilder.IsConfigured)
            //    {
            //        var configuration = new ConfigurationBuilder()
            //            .SetBasePath(Directory.GetCurrentDirectory())
            //            .AddJsonFile("appsettings.json")
            //            .Build();

            //        var connectionString = configuration.GetConnectionString("DefaultConnection");

            //        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 41)));
            //    }
            //}
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(p => p.LastName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Address).HasMaxLength(200);
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Role).HasConversion<string>().IsRequired();

            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Category>()
                        .HasMany(c => c.Products)
                        .WithOne(c => c.Category)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(c => c.Name).HasMaxLength(50).IsRequired();
                entity.Property(c => c.PhoneNumber).HasMaxLength(15).IsRequired();
                entity.Property(c => c.Location).HasMaxLength(255).IsRequired();
                entity.Property(c => c.Rate).HasPrecision(3, 2).HasDefaultValue(0.0f);
                entity.HasMany(c => c.Products)
                      .WithOne(c => c.Company)
                      .HasForeignKey(c => c.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.OrderHistory).HasMaxLength(500);
                entity.Property(o => o.Price).HasColumnType("decimal(12,2)").IsRequired();

                entity.HasOne(o => o.Company)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

              
                 entity.HasOne(o => o.Cart)
                       .WithOne(c => c.Order)
                       .HasForeignKey<Order>(o => o.CartId)
                       .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal(12,2)").IsRequired();
                entity.Property(p => p.EnergyInWatt).HasColumnType("decimal(12,2)");
                entity.Property(p => p.Image).IsRequired();

                entity.HasOne(p => p.Company)
                      .WithMany(c => c.Packages)
                      .HasForeignKey(p => p.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserProperty>(entity =>
            {
                entity.Property(c => c.SurfaceArea).HasMaxLength(50).IsRequired();
                entity.Property(c => c.Location).HasMaxLength(255).IsRequired();
                // entity.Property(po => po.PackagePrice).HasColumnType("decimal(12,2)");
                entity.Property(p => p.TotalYearsGuarantee).IsRequired();
                entity.Ignore(p => p.SavingCost);
                entity.Ignore(p => p.ROIYears);
                entity.Ignore(p => p.PricePerYear);
                entity.Ignore(p => p.ElectricityUsageAverage);
            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Stock).IsRequired();
                entity.Property(p => p.Amount).IsRequired();
                entity.Property(p => p.Price).HasColumnType("decimal(12,2)").IsRequired();
                entity.Property(p => p.Image).IsRequired();

                entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            //entity.HasOne(p => p.Company)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(p => p.CompanyId)
            //    .OnDelete(DeleteBehavior.Cascade);
        });

            modelBuilder.Entity<ProductOrder>().HasKey(pp=>new {pp.OrderId, pp.ProductId});
            modelBuilder.Entity<ProductPackage>().HasKey(pp=> new {pp.PackageId, pp.ProductId});
            modelBuilder.Entity<UserFeedBack>().Property(f => f.Content).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<UserSupport>().Property(s => s.Subject).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<UserSupport>().Property(s => s.Response).HasMaxLength(255);

            modelBuilder.Entity<Cart>()
                         .HasMany(t => t.CartItems) //one Cart has many CartItems, and each CartItem belongs to one Cart
                         .WithOne(c => c.Cart)
                         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                        .HasOne(c => c.Customer)
                        .WithMany()
                        .HasForeignKey(c => c.CustomerId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                        .HasOne(p => p.Product)
                        .WithMany()
                        .HasForeignKey(p => p.ProductId)
                        .OnDelete(DeleteBehavior.Restrict);
        }

    }
}