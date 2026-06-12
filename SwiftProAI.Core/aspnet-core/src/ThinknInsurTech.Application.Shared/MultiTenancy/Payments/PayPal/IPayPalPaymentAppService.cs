using System.Threading.Tasks;
using Abp.Application.Services;
using ThinknInsurTech.MultiTenancy.Payments.PayPal.Dto;

namespace ThinknInsurTech.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
