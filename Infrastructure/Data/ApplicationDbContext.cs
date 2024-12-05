using Application.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Engineer> engineers { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Package> packages { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<Support> supports { get; set; }
    }
}
