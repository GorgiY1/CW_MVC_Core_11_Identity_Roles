namespace CW_MVC_Core_10_Auth2
{
    public static class RolesModerator
    {
        private static List<string> _roles = new List<string>();

        public static IEnumerable<string> Roles => _roles;

        public static void AddRole(string roleName)
        {
            if (!_roles.Contains(roleName))
            {
                _roles.Add(roleName);
            }
        }

        public static bool RoleExists(string roleName) => _roles.Contains(roleName);
    }
}
