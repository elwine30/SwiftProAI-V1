using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using ThinknInsurTech.Configuration;

namespace ThinknInsurTech.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(ThinknInsurTechTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
