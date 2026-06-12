using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ThinknInsurTech.Web.Public.Views
{
    public abstract class ThinknInsurTechRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ThinknInsurTechRazorPage()
        {
            LocalizationSourceName = ThinknInsurTechConsts.LocalizationSourceName;
        }
    }
}
