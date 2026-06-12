using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Branches;

namespace ThinknInsurTech.Branches
{
    public class BranchManager : ThinknInsurTechDomainServiceBase, IBranchManager
    {
        private readonly IRepository<Branch, int> _branchRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public BranchManager(
            IRepository<Branch, int> branchRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _branchRepository = branchRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<int> CreateBranchAsync(Branch casetype)
        {
            int id = default;
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                id = await _branchRepository.InsertAndGetIdAsync(casetype);
                await CurrentUnitOfWork.SaveChangesAsync();
            });
            return id;
        }

        public async Task<List<Branch>> GetAllBranchAsync()
        {
            var details = await _branchRepository.GetAll()
                                   .Where(m => m.Id > 0)
                                   .OrderByDescending(m => m.Id)
                                   .ToListAsync();

            return details;
        }

        public async Task<Branch> GetBranchbyIdAsync(int branchId)
        {
            var details = await _branchRepository.GetAsync(branchId);

            return details;
        }

    }
}
