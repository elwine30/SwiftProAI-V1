using System.Threading.Tasks;
using ThinknInsurTech.Sessions.Dto;

namespace ThinknInsurTech.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
