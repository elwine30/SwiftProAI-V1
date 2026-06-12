using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Case;
using Microsoft.EntityFrameworkCore;

namespace ThinknInsurTech.Case
{
    public class CaseTypeManager : ThinknInsurTechDomainServiceBase, ICaseTypeManager
    {
        private readonly IRepository<CaseType, int> _caseTypeRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CaseTypeManager(
            IRepository<CaseType, int> caseTypeRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _caseTypeRepository = caseTypeRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<int> CreateCaseTypeAsync(CaseType casetype)
        {
            int id = default;
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                id = await _caseTypeRepository.InsertAndGetIdAsync(casetype);
                await CurrentUnitOfWork.SaveChangesAsync();
            });
            return id;
        }

        public async Task<List<CaseType>> GetAllCaseTypeAsync()
        {
            var details = await _caseTypeRepository.GetAll()
                                   .Where(m => m.Id > 0)
                                   .OrderByDescending(m => m.Id)
                                   .ToListAsync();

            return details;
        }

        public async Task<CaseType> GetCaseTypebyIdAsync(int caseTypeId)
        {
            var details = await _caseTypeRepository.GetAsync(caseTypeId);

            return details;
        }

    }
}