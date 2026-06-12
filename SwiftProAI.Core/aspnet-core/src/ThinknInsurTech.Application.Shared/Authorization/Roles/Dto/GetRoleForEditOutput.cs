using System.Collections.Generic;
using ThinknInsurTech.Authorization.Permissions.Dto;

namespace ThinknInsurTech.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}