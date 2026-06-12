using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseAdjustersAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseAdjusterForViewDto>> GetAll(GetAllCaseAdjustersInput input);

        Task<GetCaseAdjusterForViewDto> GetCaseAdjusterForView(int registerId);

        Task<GetCaseAdjusterForEditOutput> GetCaseAdjusterForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseAdjusterDto input);

        Task<List<CaseAdjusterScopeAssignmentLookupTableDto>> GetAllScopeAssignmentForTableDropdown();

        Task<List<CaseAdjusterLocationLookupTableDto>> GetAllStateLocationForTableDropdown(int parentId);

        Task<List<CaseAdjusterUserLookupTableDto>> GetAllEditorUserForTableDropdown();

        Task<List<CaseAdjusterLookupTableDto>> GetAllCaseTypeForTableDropdown();

    }
}