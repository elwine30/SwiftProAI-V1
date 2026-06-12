using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThinknInsurTech
{
    public class ThinknInsurTechCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechCoreSharedModule).GetAssembly());
        }
    }
}