using Abp.Authorization;
using FeeSystem.Authorization.Roles;
using FeeSystem.Authorization.Users;

namespace FeeSystem.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
