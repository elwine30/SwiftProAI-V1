using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Integration.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Integration
{
    public interface IOpenAIIntegrationLogsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOpenAIIntegrationLogForViewDto>> GetAll(GetAllOpenAIIntegrationLogsInput input);

    }
}