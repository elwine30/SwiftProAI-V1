using System.Threading.Tasks;
using Abp.Application.Services;
using ThinknInsurTech.Install.Dto;

namespace ThinknInsurTech.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}