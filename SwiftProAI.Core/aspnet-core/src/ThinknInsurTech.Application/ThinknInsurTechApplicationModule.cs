using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ThinknInsurTech.Authorization;

namespace ThinknInsurTech
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(ThinknInsurTechApplicationSharedModule),
        typeof(ThinknInsurTechCoreModule)
        )]
    public class ThinknInsurTechApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechApplicationModule).GetAssembly());
        }
    }
}