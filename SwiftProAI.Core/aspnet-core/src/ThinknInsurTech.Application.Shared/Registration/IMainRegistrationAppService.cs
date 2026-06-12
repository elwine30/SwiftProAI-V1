using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface IMainRegistrationAppService : IApplicationService
    {
        Task<PagedResultDto<MainRegistrationDashboardDto>> GetMainRegistrationDetails(GetMainRegistrationDetailsInput input);

        Task<Dictionary<int, int>> GetMainRegistrationDashboardSummary();

        Task<int> CreateMainRegistration(CreateMainRegistrationInput registration);

        Task UpdateStatus(int registerId);

        MainRegistrationDto GetMainRegistrationDetailsByRegisterId(int registerId);

        Task UpdateCaseCompany(ReassignCaseCompanyDto data);

        Task UpdateCaseAdjuster(ReassignCaseAdjusterDto data);

        Task<List<RegistrationCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown();


        // GetMainRegistrationMinMaxCreationTime - Returns MainRegistration Min Max Creation Time to populate list of years/months
        Task<RegistrationCreationTimeMinMax> GetMainRegistrationMinMaxCreationTime();

        Task<string> GetMainRegistrationFileRefNo(int registerId);

    }
}

