using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Approval.Dtos;
using ThinknInsurTech.Dto;

using System.Collections.Generic;

namespace ThinknInsurTech.Approval
{
    public interface IViewThirdPartyCaseRequestsAppService : IApplicationService
    {
        Task<PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>> GetAllNotOnboarded(GetAllViewThirdPartyCaseRequestsInput input);

        Task<GetViewThirdPartyCaseRequestForViewDto> GetViewThirdPartyCaseRequestForView(int id);

        Task<GetViewThirdPartyCaseRequestForEditOutput> GetViewThirdPartyCaseRequestForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditViewThirdPartyCaseRequestDto input);

        Task Delete(EntityDto input);

    }
}