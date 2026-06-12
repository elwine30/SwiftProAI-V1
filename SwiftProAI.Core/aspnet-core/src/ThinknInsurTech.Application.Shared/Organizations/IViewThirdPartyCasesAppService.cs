using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Organizations.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Organizations
{
    public interface IViewThirdPartyCasesAppService : IApplicationService
    {
        Task<PagedResultDto<GetViewThirdPartyCasesForViewDto>> GetAll(GetAllViewThirdPartyCasesInput input);

        Task<GetViewThirdPartyCasesForEditOutput> GetViewThirdPartyCasesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditViewThirdPartyCaseDto input);

        Task Delete(EntityDto input);

    }
}