using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;
using ThinknInsurTech.Reports.Dto;

namespace ThinknInsurTech.Reports
{
    public interface IWIPSummaryReportsAppService : IApplicationService
    {
        Task<GetAllWipSummaryDataDto> GetAllWipSummaryData(WipSummaryDataInputDto input);
        Task<PagedResultDto<WIPSummaryReportCaseTypeLookupTableDto>> GetAllCaseTypeForLookupTable(GetAllForLookupTableInput input);
        Task<PagedResultDto<WIPSummaryReportUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
        Task<PagedResultDto<WIPSummaryReportCompanyLookupTableDto>> GetAllCompanyForLookupTable(GetAllForLookupTableInput input);
        Task<FileDto> GetWIPSummaryReportsToExcel(WipSummaryDataInputDto input);
    }
}