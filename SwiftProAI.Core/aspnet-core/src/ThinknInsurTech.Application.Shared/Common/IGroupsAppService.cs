using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;

namespace ThinknInsurTech.Common
{
    public interface IGroupsAppService : IApplicationService
    {
        Task<PagedResultDto<GetGroupForViewDto>> GetAll(GetAllGroupsInput input);

        Task<GetGroupForViewDto> GetGroupForView(int id);

        Task<GetGroupForEditOutput> GetGroupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGroupDto input);

        Task Delete(EntityDto input);


    }
}