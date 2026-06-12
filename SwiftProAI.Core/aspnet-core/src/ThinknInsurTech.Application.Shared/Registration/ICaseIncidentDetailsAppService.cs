using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseIncidentDetailsAppService : IApplicationService
    {

        Task<GetCaseIncidentDetailForEditOutput> GetCaseIncidentDetailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseIncidentDetailDto input);
        Task<GetCaseIncidentDetailForViewDto> GetCaseIncidentDetailForView(int id);

        Task Delete(EntityDto input);

        Task<List<CaseIncidentDetailMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

        Task RemoveCircumstancesFileUploadFile(EntityDto input);
        Task<GetCaseIncidentDetailForEditOutput> GetOneData(int mainId);


    }
}