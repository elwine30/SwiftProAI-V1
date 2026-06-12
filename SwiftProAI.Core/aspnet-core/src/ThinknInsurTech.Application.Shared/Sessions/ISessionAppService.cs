using System.Threading.Tasks;
using Abp.Application.Services;
using ThinknInsurTech.Sessions.Dto;

namespace ThinknInsurTech.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
