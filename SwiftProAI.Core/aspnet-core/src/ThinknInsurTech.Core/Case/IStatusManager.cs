using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThinknInsurTech.Case
{
    public interface IStatusManager : IDomainService
    {
        Task<int> CreateStatusAsync(Status casetype);

        Task<List<Status>> GetAllStatusAsync();

        Task<Status> GetStatusbyIdAsync(int statusId);

    }
}
