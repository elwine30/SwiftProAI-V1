using Abp.Application.Services;
using System.Threading.Tasks;
using ThinknInsurTech.Organizations.Dto;

namespace ThinknInsurTech.Organizations
{
    public interface IOUOnboardingAppService : IApplicationService
    {
        Task CreateOnboardingOu(CreateOUOnboardingInput input);
    }
}
