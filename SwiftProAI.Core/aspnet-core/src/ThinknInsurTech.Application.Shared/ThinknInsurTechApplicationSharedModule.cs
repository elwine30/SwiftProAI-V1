using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThinknInsurTech
{
    [DependsOn(typeof(ThinknInsurTechCoreSharedModule))]
    public class ThinknInsurTechApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechApplicationSharedModule).GetAssembly());
        }
    }
}