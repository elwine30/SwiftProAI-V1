using System.Collections.Generic;
using ThinknInsurTech.Authorization.Permissions.Dto;

namespace ThinknInsurTech.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}