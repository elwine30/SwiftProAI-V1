using System.Threading.Tasks;
using Abp.Application.Services;
using ThinknInsurTech.MultiTenancy.Payments.Dto;
using ThinknInsurTech.MultiTenancy.Payments.Stripe.Dto;

namespace ThinknInsurTech.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}