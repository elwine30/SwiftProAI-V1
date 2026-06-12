using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Reports
{
    public interface IInvoiceReportsAppService : IApplicationService
    {
        Task<PagedResultDto<GetInvoiceReportForViewDto>> GetAll(GetAllInvoiceReportsInput input);
        Task<FileDto> GetInvoiceReportsToExcel(GetAllInvoiceReportsForExcelInput input);

    }
}