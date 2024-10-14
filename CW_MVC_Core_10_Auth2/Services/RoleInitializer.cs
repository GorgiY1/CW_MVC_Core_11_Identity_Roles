using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class RoleInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string roleName = "Administrator";
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var role = new IdentityRole(roleName)
            {
                ConcurrencyStamp = Guid.NewGuid().ToString() // Set the concurrency stamp
            };
            await roleManager.CreateAsync(role);
        }
    }
}