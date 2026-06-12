using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Organizations
{
    public interface IViewThirdPartyCasesManager : IDomainService
    {
        Task CreateMainRegistrationOU(ViewThirdPartyCases input);
        Task UpdateMainRegistrationOU(long assignedOUId, int registerId, long newAssignedOUId);
        Task<ViewThirdPartyCases> GetMainRegistrationOUByAssignedOUIdAsync(long assignedOUId, int registerId);
        Task DeleteMainRegistrationOU(long? ouId, int registerId);
    }
}
