using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;

namespace ThinknInsurTech.Common
{
    public interface IStaffsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStaffForViewDto>> GetAll(GetAllStaffsInput input);

        Task<GetStaffForViewDto> GetStaffForView(int id);

        Task<GetStaffForEditOutput> GetStaffForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStaffDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<StaffGroupLookupTableDto>> GetAllGroupForLookupTable(GetAllForLookupTableInput input);

        Task<List<StaffGroupLookupTableDto>> GetAllGroupForTableDropdown();

    }
}