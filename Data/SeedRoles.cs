using Microsoft.AspNetCore.Identity;

namespace EcoPowerHub.Data
{
    public class SeedRoles
    {
       public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Client", "Company" };
            foreach (var role in roles) 
                if(! await roleManager.RoleExistsAsync(role)) 
                    await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
