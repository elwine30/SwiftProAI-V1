using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Reports
{
    public interface ICaseReportsAppService : IApplicationService
    {
        Task<GetCaseReportForViewDto> GetAll(GetAllCaseReportsInput input);

        Task<FileDto> GetCaseReportsToExcel(GetAllCaseReportsForExcelInput input);

    }
}