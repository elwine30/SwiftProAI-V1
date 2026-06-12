using Abp.AspNetCore.Mvc.ViewComponents;

namespace ThinknInsurTech.Web.Public.Views
{
    public abstract class ThinknInsurTechViewComponent : AbpViewComponent
    {
        protected ThinknInsurTechViewComponent()
        {
            LocalizationSourceName = ThinknInsurTechConsts.LocalizationSourceName;
        }
    }
}