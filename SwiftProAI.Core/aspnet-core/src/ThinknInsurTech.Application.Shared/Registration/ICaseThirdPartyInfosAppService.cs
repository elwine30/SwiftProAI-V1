using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseThirdPartyInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAll(GetAllCaseThirdPartyInfosInput input);
        Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAllForView(GetAllCaseThirdPartyInfosInput input);

        Task<GetCaseThirdPartyInfoForEditOutput> GetCaseThirdPartyInfoForEdit(EntityDto input);

        Task<bool?> CreateOrEdit(CreateOrEditCaseThirdPartyInfoDto input);

        Task Delete(EntityDto input);
        Task RemoveTpiFile(RemoveFile input);
        Task<List<CaseThirdPartyInfoMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();
    }
}