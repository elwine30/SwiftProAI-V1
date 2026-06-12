using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Reports
{
    public interface IWIPReportsAppService : IApplicationService
    {
        Task<PagedResultDto<GetWIPReportForViewDto>> GetAll(GetAllWIPReportsInput input);

        Task<FileDto> GetWIPReportsToExcel(GetAllWIPReportsForExcelInput input);

    }
}