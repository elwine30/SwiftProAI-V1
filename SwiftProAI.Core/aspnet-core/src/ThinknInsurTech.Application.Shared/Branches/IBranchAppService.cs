using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Branches.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Branches
{
    public interface IBranchAppService : IApplicationService
    {
        Task<PagedResultDto<GetBranchForViewDto>> GetAll(GetAllBranchInput input);

        Task<GetBranchForViewDto> GetBranchForView(int id);

        Task<GetBranchForEditOutput> GetBranchForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBranchDto input);

        Task Delete(EntityDto input);

    }
}