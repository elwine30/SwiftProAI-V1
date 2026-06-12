using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ThinknInsurTech.Configure;
using ThinknInsurTech.Startup;
using ThinknInsurTech.Test.Base;

namespace ThinknInsurTech.GraphQL.Tests
{
    [DependsOn(
        typeof(ThinknInsurTechGraphQLModule),
        typeof(ThinknInsurTechTestBaseModule))]
    public class ThinknInsurTechGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThinknInsurTechGraphQLTestModule).GetAssembly());
        }
    }
}