using Abp.Auditing;
using ThinknInsurTech.Configuration.Dto;

namespace ThinknInsurTech.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}