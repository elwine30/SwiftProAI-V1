using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Reports.Dto;

using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Reports
{
    public interface IAdjusterReportsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAdjusterReportForViewDto>> GetAll(GetAllAdjusterReportsInput input);
        Task<FileDto> GetAdjusterReportsToExcel(GetAllAdjusterReportsForExcelInput input);

        Task<PagedResultDto<AdjusterReportUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}