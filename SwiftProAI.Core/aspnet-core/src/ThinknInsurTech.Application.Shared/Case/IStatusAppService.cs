using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using ThinknInsurTech.Case.Dto;

namespace ThinknInsurTech.Case
{
    public interface IStatusAppService : IApplicationService
    {
        Task<StatusDto> GetStatusDetailsbyId(int id);

        Task<ListResultDto<StatusDto>> GetAllStatusDetails();

        Task<int> CreateStatus(StatusDto input);
    }
}
