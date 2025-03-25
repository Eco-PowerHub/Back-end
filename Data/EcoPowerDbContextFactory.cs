using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EcoPowerHub.Data
{
    public class EcoPowerDbContextFactory : IDesignTimeDbContextFactory<EcoPowerDbContext>
    {
        public EcoPowerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EcoPowerDbContext>();
            // Replace with your MySQL connection string
            optionsBuilder.UseMySql(
                "Server=ecopower.czyqc6cssjrd.me-south-1.rds.amazonaws.com;Port=3306;Database=ecopower;User=admin;Password=Eco%Power123;",
                ServerVersion.AutoDetect("Server=ecopower.czyqc6cssjrd.me-south-1.rds.amazonaws.com;Port=3306;Database=ecopower;User=admin;Password=Eco%Power123;")
            );

            return new EcoPowerDbContext(optionsBuilder.Options);
        }
    }
}
