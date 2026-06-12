using ThinknInsurTech.Authorization.Users.Dto;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Organizations.Dto
{
    public class CreateOUOnboardingInput
    {
        public CreateOrganizationUnitInput OrganizationDto { get; set; }
        public CreateOrEditDocumentSettingDto DocumentSettingDto { get; set; }
        public CreateOrUpdateUserInput AdminDto { get; set; }
    }
}
