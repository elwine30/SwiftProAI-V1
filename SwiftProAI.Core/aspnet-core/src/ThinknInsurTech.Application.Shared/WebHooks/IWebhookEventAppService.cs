using System.Threading.Tasks;
using Abp.Webhooks;

namespace ThinknInsurTech.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
