using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Branches;

namespace ThinknInsurTech.Branches
{
    public interface IBranchManager : IDomainService
    {
        Task<int> CreateBranchAsync(Branch branch);

        Task<List<Branch>> GetAllBranchAsync();

        Task<Branch> GetBranchbyIdAsync(int branchId);

    }
}
