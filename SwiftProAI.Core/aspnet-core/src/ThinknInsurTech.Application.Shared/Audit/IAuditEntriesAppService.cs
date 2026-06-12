using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Audit
{
    public interface IAuditEntriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAuditEntryForViewDto>> GetAll(GetAllAuditEntriesInput input);

        Task<FileDto> GetAuditEntriesToExcel(GetAllAuditEntriesForExcelInput input);

    }
}