using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThinknInsurTech
{
    public class ThinknInsurTechClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechClientModule).GetAssembly());
        }
    }
}
