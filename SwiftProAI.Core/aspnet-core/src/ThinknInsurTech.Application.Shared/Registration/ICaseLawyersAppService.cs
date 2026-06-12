using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseLawyersAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAll(GetAllCaseLawyersInput input);
        Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAllForView(GetAllCaseLawyersInput input);


        //Task<GetCaseLawyerForViewDto> GetCaseLawyerForView(int id);

        Task<GetCaseLawyerForEditOutput> GetCaseLawyerForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseLawyerDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<CaseLawyerMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(Registration.Dtos.GetAllForLookupTableInput input);

        Task<List<CaseLawyerLawFirmLookupTableDto>> GetAllLawFirmForTableDropdown();

    }
}