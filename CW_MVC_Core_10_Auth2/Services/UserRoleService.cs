using CW_MVC_Core_10_Auth2.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRoleService : IUserRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public UserRoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<bool> AddRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return false;

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        return await _userManager.GetRolesAsync(user);
    }
}
