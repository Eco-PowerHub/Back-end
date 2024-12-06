using Microsoft.AspNetCore.Identity;

namespace EcoPower.Data
{
    public class SeedRoler
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };
            bool IsRoleSeeded = await roleManager.RoleExistsAsync("Admin");
            foreach (string role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }   
        }
    }
}
