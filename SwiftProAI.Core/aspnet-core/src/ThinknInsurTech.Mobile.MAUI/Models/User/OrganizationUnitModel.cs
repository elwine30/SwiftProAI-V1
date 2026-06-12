using Abp.AutoMapper;
using ThinknInsurTech.Organizations.Dto;

namespace ThinknInsurTech.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}
