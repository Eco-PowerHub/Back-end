using EcoPower.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EcoPower.Data
{
    public class EcoPowerDbContext :IdentityDbContext<ApplicationUser>
    {
        public EcoPowerDbContext(DbContextOptions<EcoPowerDbContext> options) :base(options) { }

        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Engineer> engineers { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Package> packages { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Models.Property> properties { get; set; }
        public DbSet<Support> supports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           // builder.Entity<ApplicationUser>().Property(p => p.FullName).HasMaxLength(50).IsRequired();
            builder.Entity<ApplicationUser>().Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            builder.Entity<ApplicationUser>().Property(p => p.LastName).HasMaxLength(50).IsRequired();
            builder.Entity<ApplicationUser>().Property(p => p.Address).HasMaxLength(50);

            builder.Entity<Support>().Property(p => p.Subject).HasMaxLength(255).IsRequired();
            builder.Entity<Support>().Property(p => p.ClientId).HasMaxLength(255).IsRequired();

            builder.Entity<Models.Property>(entity =>
            {
                entity.Property(p => p.SurfaceArea)
                    .IsRequired()
                    .HasPrecision(10, 3)
                    .HasComment("Surface area of the property in square meters.");

                entity.Property(p => p.Location)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The location of the property.");

                entity.Property(p => p.Description)
                    .HasMaxLength(1000)
                    .HasComment("Detailed Description please!");

                entity.Property(p => p.ElectricityUsage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Electricity usage percentage or amount");

                entity.Property(p => p.ClientId)
                    .IsRequired();

                entity.HasOne(p => p.Client)
                    .WithMany()
                    .HasForeignKey(p => p.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Feedback>(entity =>
            {
                entity.Property(f => f.Rating)
                    .IsRequired()
                    .HasComment("Rating from 1 to 5!");

                entity.Property(f => f.Content)
                    .IsRequired(false)
                    .HasMaxLength(1000)
                    .HasComment("Extra Content for feedback!");

                entity.Property(f => f.ClientId)
                    .IsRequired();

                entity.HasOne(f => f.Client)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Amount)
                    .IsRequired()
                    .HasPrecision(18, 2)
                    .HasComment("Payment amount, must be a positive value.");

                entity.Property(p => p.OrderId)
                    .IsRequired();

                entity.HasOne(p => p.Order)
                    .WithMany()
                    //.HasForeignKey(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Order>()
                .HasKey(o => new { o.ProductId, o.ClientId });

            builder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasOne(p => p.Package)
                .WithMany()
                .HasForeignKey(p => p.PackageId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Package>()
                .HasMany(p => p.Products)
                .WithOne()
                .HasForeignKey(p => p.PackageId);

            builder.Entity<Support>()
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Support>()
                .HasOne(s => s.Engineer)
                .WithMany()
                .HasForeignKey(s => s.EngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Support>()
                .Property(s => s.ClientId)
                .HasMaxLength(450)  
                .IsRequired();

        }


    }
}

