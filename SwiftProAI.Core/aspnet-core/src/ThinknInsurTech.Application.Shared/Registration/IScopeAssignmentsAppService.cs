using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Registration
{
    public interface IScopeAssignmentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetScopeAssignmentForViewDto>> GetAll(GetAllScopeAssignmentsInput input);

        Task<GetScopeAssignmentForViewDto> GetScopeAssignmentForView(int id);

        Task<GetScopeAssignmentForEditOutput> GetScopeAssignmentForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditScopeAssignmentDto input);

        Task Delete(EntityDto input);

    }
}