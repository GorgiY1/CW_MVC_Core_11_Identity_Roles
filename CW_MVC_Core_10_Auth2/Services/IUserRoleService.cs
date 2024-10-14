namespace CW_MVC_Core_10_Auth2.Services
{
    public interface IUserRoleService
    {
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    }
}
