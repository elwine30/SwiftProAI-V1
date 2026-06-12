using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.HealthChecks
{
    public class ThinknInsurTechDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public ThinknInsurTechDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("ThinknInsurTechDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("ThinknInsurTechDbContext could not connect to database"));
        }
    }
}
