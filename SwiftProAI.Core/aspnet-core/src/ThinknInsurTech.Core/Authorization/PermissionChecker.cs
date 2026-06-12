using Abp.Authorization;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users;

namespace ThinknInsurTech.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
