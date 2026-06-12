using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Common
{
    public interface ILocationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLocationForViewDto>> GetAll(GetAllLocationsInput input);

        Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLocationDto input);

        Task Delete(EntityDto input);

    }
}