using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThinknInsurTech.Companies
{
    public interface ICompanyManager : IDomainService
    {
        Task<int> CreateCompanyAsync(InsuranceCompany company);

        Task<List<InsuranceCompany>> GetAllCompanyAsync();

        Task<InsuranceCompany> GetCompanybyIdAsync(int companyId);
    }
}
