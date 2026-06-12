using System.Threading.Tasks;
using Abp.Domain.Services;

namespace ThinknInsurTech.Registration
{
    public interface IMainRegistrationManager : IDomainService
    {
        Task<int> CreateMainRegistrationAsync(MainRegistration registration);

    }
}
