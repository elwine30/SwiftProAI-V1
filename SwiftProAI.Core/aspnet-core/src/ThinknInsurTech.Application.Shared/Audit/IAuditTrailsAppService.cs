using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Audit
{
    public interface IAuditTrailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAuditTrailForViewDto>> GetAll(GetAllAuditTrailsInput input);

        Task<GetAuditTrailForViewDto> GetAuditTrailForView(int id);

        Task<GetAuditTrailForEditOutput> GetAuditTrailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAuditTrailDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAuditTrailsToExcel(GetAllAuditTrailsForExcelInput input);

    }
}