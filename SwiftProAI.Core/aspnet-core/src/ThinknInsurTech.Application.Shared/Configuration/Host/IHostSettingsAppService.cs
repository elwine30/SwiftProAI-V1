using System.Threading.Tasks;
using Abp.Application.Services;
using ThinknInsurTech.Configuration.Host.Dto;

namespace ThinknInsurTech.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
