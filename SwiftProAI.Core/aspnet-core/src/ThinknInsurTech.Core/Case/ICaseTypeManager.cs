using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThinknInsurTech.Case
{
    public interface ICaseTypeManager : IDomainService
    {
        Task<int> CreateCaseTypeAsync(CaseType casetype);

        Task<List<CaseType>> GetAllCaseTypeAsync();

        Task<CaseType> GetCaseTypebyIdAsync(int caseTypeId);

    }
}
