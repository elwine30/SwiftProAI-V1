using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICasePoliceReportsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCasePoliceReportForViewDto>> GetAll(GetAllCasePoliceReportsInput input);
        Task<PagedResultDto<GetCasePoliceReportForViewDto>> GetAllForView(GetAllCasePoliceReportsInput input);

        Task<GetCasePoliceReportForViewDto> GetCasePoliceReportForView(int id);

        Task<GetCasePoliceReportForEditOutput> GetCasePoliceReportForEdit(GetCasePoliceReportForEditInput input);

        Task CreateOrEdit(CreateOrEditCasePoliceReportDto input);

        Task Delete(EntityDto input);

        Task<List<CasePoliceReportMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

        Task RemoveReportFileUploadFile(EntityDto input);

    }
}