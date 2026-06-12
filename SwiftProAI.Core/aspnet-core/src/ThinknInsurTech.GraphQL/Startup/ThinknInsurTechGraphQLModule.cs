using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThinknInsurTech.Startup
{
    [DependsOn(typeof(ThinknInsurTechCoreModule))]
    public class ThinknInsurTechGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}