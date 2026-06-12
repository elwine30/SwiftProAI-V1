using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ThinknInsurTech.ApiClient;
using ThinknInsurTech.Mobile.MAUI.Core.ApiClient;

namespace ThinknInsurTech
{
    [DependsOn(typeof(ThinknInsurTechClientModule), typeof(AbpAutoMapperModule))]

    public class ThinknInsurTechMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechMobileMAUIModule).GetAssembly());
        }
    }
}