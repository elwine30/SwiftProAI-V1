using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICaseWorkshopsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseWorkshopForViewDto>> GetAll(GetAllCaseWorkshopsInput input);

        //Task<GetCaseWorkshopForViewDto> GetCaseWorkshopForView(int id);

        Task<GetCaseWorkshopForEditOutput> GetCaseWorkshopForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseWorkshopDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<CaseWorkshopMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(GetAllForLookupTableInput input);

        Task<List<CaseWorkshopWorkshopLookupTableDto>> GetAllWorkshopForTableDropdown();

        Task<GetCaseWorkshopForViewDto> GetCaseWorkshopForView(EntityDto input);

    }
}