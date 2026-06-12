using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using ThinknInsurTech.Remarks.Dto;

namespace ThinknInsurTech.Remarks
{
    public interface IRemarkAppService : IApplicationService
    {
        Task<RemarkDto> GetRemarkDetailsbyId(int id);

        Task<ListResultDto<RemarkDto>> GetAllRemarkDetails();

        Task<int> CreateRemark(RemarkDto input);
    }
}
