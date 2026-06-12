using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Users;

namespace ThinknInsurTech.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
