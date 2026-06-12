using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseSearchFeesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseSearchFeeForViewDto>> GetAll(GetAllCaseSearchFeesInput input);

        Task<GetCaseSearchFeeForEditOutput> GetCaseSearchFeeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseSearchFeeDto input);

        Task Delete(EntityDto input);

        Task<List<CaseSearchFeeDto>> GetCaseSearchFeeAmountByRegisterId(EntityDto input);

    }
}