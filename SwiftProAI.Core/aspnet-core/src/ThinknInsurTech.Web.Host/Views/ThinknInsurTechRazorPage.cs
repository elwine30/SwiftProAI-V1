using Abp.AspNetCore.Mvc.Views;

namespace ThinknInsurTech.Web.Views
{
    public abstract class ThinknInsurTechRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected ThinknInsurTechRazorPage()
        {
            LocalizationSourceName = ThinknInsurTechConsts.LocalizationSourceName;
        }
    }
}
