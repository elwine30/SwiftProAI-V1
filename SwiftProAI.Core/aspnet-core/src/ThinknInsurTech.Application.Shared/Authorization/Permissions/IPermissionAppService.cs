using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization.Permissions.Dto;

namespace ThinknInsurTech.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
