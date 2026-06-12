using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Vehicles.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Vehicles
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Vehicles)]
    public class VehiclesAppService : ThinknInsurTechAppServiceBase, IVehiclesAppService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehiclesAppService(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;

        }

        public virtual async Task<PagedResultDto<GetVehicleForViewDto>> GetAll(GetAllVehiclesInput input)
        {

            var filteredVehicles = _vehicleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Make.Contains(input.Filter) || e.Model.Contains(input.Filter) || e.Specification.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MakeFilter), e => e.Make.Contains(input.MakeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ModelFilter), e => e.Model.Contains(input.ModelFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SpecificationFilter), e => e.Specification.Contains(input.SpecificationFilter));

            var pagedAndFilteredVehicles = filteredVehicles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vehicles = from o in pagedAndFilteredVehicles
                           select new
                           {

                               o.Make,
                               o.Model,
                               o.Specification,
                               Id = o.Id
                           };

            var totalCount = await filteredVehicles.CountAsync();

            var dbList = await vehicles.ToListAsync();
            var results = new List<GetVehicleForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVehicleForViewDto()
                {
                    Vehicle = new VehicleDto
                    {

                        Make = o.Make,
                        Model = o.Model,
                        Specification = o.Specification,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVehicleForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetVehicleForViewDto> GetVehicleForView(int id)
        {
            var vehicle = await _vehicleRepository.GetAsync(id);

            var output = new GetVehicleForViewDto { Vehicle = ObjectMapper.Map<VehicleDto>(vehicle) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Vehicles_Edit)]
        public virtual async Task<GetVehicleForEditOutput> GetVehicleForEdit(EntityDto input)
        {
            var vehicle = await _vehicleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVehicleForEditOutput { Vehicle = ObjectMapper.Map<CreateOrEditVehicleDto>(vehicle) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditVehicleDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Vehicles_Create)]
        protected virtual async Task Create(CreateOrEditVehicleDto input)
        {
            var vehicle = ObjectMapper.Map<Vehicle>(input);

            await _vehicleRepository.InsertAsync(vehicle);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Vehicles_Edit)]
        protected virtual async Task Update(CreateOrEditVehicleDto input)
        {
            var vehicle = await _vehicleRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, vehicle);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Vehicles_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _vehicleRepository.DeleteAsync(input.Id);
        }

    }
}