using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Vehicles.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Vehicles
{
    public interface IVehiclesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVehicleForViewDto>> GetAll(GetAllVehiclesInput input);

        Task<GetVehicleForViewDto> GetVehicleForView(int id);

        Task<GetVehicleForEditOutput> GetVehicleForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditVehicleDto input);

        Task Delete(EntityDto input);

    }
}