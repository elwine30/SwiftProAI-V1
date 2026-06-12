using Abp.Application.Services;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Logging.Dto;

namespace ThinknInsurTech.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
