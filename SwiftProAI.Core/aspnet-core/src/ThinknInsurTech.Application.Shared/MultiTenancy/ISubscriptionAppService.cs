using System.Threading.Tasks;
using Abp.Application.Services;
using ThinknInsurTech.MultiTenancy.Dto;
using ThinknInsurTech.MultiTenancy.Payments.Dto;

namespace ThinknInsurTech.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
        
        Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);
        
        Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);
        
        Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
    }
}
