using Abp.Domain.Services;

namespace ThinknInsurTech
{
    public abstract class ThinknInsurTechDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected ThinknInsurTechDomainServiceBase()
        {
            LocalizationSourceName = ThinknInsurTechConsts.LocalizationSourceName;
        }
    }
}
