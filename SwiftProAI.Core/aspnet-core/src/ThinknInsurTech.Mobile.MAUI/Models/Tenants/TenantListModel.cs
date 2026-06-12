using Abp.AutoMapper;
using ThinknInsurTech.MultiTenancy.Dto;

namespace ThinknInsurTech.Mobile.MAUI.Models.Tenants
{
    [AutoMapFrom(typeof(TenantListDto))]
    [AutoMapTo(typeof(TenantEditDto), typeof(CreateTenantInput))]
    public class TenantListModel : TenantListDto
    {
 
    }
}
