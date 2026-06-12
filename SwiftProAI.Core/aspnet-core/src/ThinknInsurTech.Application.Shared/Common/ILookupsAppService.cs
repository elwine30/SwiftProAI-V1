using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dto;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    public interface ILookupsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLookupForViewDto>> GetAll(GetAllLookupsInput input);

        Task<GetLookupForViewDto> GetLookupForView(int id);

        Task CreateOrEdit(CreateOrEditLookupDto input);

        Task Delete(EntityDto input);
        List<GetLookupByGroupDto> GetByGroup(string group);


    }
}