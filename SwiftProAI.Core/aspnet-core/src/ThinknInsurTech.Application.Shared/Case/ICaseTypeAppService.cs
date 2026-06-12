using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using ThinknInsurTech.Case.Dto;

namespace ThinknInsurTech.Case
{
    public interface ICaseTypeAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseTypeForViewDto>> GetAll(GetAllCaseTypeInput input);

        Task<GetCaseTypeForViewDto> GetCaseTypeForView(int id);

        Task<GetCaseTypeForEditOutput> GetCasetypeForEdit(EntityDto input);

        Task<CaseTypeDto> GetCaseTypeDetailsbyId(int id);

        Task<ListResultDto<CaseTypeDto>> GetAllCaseTypeDetails();

        Task<int> CreateCaseType(CaseTypeDto input);
    }
}
