using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Companies;

namespace ThinknInsurTech.Companies
{
    public class CompanyManager : ThinknInsurTechDomainServiceBase, ICompanyManager
    {
        private readonly IRepository<InsuranceCompany, int> _insuranceCompanyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CompanyManager(
            IRepository<InsuranceCompany, int> insuranceCompanyRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<int> CreateCompanyAsync(InsuranceCompany company)
        {
            int id = default;
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                id = await _insuranceCompanyRepository.InsertAndGetIdAsync(company);
                await CurrentUnitOfWork.SaveChangesAsync();
            });
            return id;
        }

        public async Task<List<InsuranceCompany>> GetAllCompanyAsync()
        {
            var details = await _insuranceCompanyRepository.GetAll()
                                   .Where(m => m.Id > 0)
                                   .OrderByDescending(m => m.Id)
                                   .ToListAsync();

            return details;
        }

        public async Task<InsuranceCompany> GetCompanybyIdAsync(int companyId)
        {
            var details = await _insuranceCompanyRepository.GetAsync(companyId);

            return details;
        }

    }
}
