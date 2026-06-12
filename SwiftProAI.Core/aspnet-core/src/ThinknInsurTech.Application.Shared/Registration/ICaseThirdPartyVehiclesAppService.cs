using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseThirdPartyVehiclesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseThirdPartyVehicleForViewDto>> GetAll(GetAllCaseThirdPartyVehiclesInput input);
        Task<PagedResultDto<GetCaseThirdPartyVehicleForViewDto>> GetAllForView(GetAllCaseThirdPartyVehiclesInput input);


        //Task<GetCaseThirdPartyVehicleForViewDto> GetCaseThirdPartyVehicleForView(int id);

        Task<GetCaseThirdPartyVehicleForEditOutput> GetCaseThirdPartyVehicleForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseThirdPartyVehicleDto input);

        Task Delete(EntityDto input);

        Task<List<CaseThirdPartyVehicleMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

    }
}