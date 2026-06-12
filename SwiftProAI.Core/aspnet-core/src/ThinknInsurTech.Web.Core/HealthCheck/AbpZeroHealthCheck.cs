using Microsoft.Extensions.DependencyInjection;
using ThinknInsurTech.HealthChecks;

namespace ThinknInsurTech.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<ThinknInsurTechDbContextHealthCheck>("Database Connection");
            builder.AddCheck<ThinknInsurTechDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
