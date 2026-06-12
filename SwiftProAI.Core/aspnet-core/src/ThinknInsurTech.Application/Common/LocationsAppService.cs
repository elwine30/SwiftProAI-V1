using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_Locations)]
    public class LocationsAppService : ThinknInsurTechAppServiceBase, ILocationsAppService
    {
        private readonly IRepository<Location> _locationRepository;

        public LocationsAppService(IRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;

        }

        public virtual async Task<PagedResultDto<GetLocationForViewDto>> GetAll(GetAllLocationsInput input)
        {

            var filteredLocations = _locationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ShortName.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortNameFilter), e => e.ShortName.Contains(input.ShortNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinParentLocationIdFilter != null, e => e.ParentLocationId >= input.MinParentLocationIdFilter)
                        .WhereIf(input.MaxParentLocationIdFilter != null, e => e.ParentLocationId <= input.MaxParentLocationIdFilter);

            var pagedAndFilteredLocations = filteredLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var locations = from o in pagedAndFilteredLocations
                            select new
                            {

                                o.ShortName,
                                o.Name,
                                o.ParentLocationId,
                                Id = o.Id
                            };

            var totalCount = await filteredLocations.CountAsync();

            var dbList = await locations.ToListAsync();
            var results = new List<GetLocationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLocationForViewDto()
                {
                    Location = new LocationDto
                    {

                        ShortName = o.ShortName,
                        Name = o.Name,
                        ParentLocationId = o.ParentLocationId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLocationForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Locations_Edit)]
        public virtual async Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input)
        {
            var location = await _locationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLocationForEditOutput { Location = ObjectMapper.Map<CreateOrEditLocationDto>(location) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditLocationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Locations_Create)]
        protected virtual async Task Create(CreateOrEditLocationDto input)
        {
            var location = ObjectMapper.Map<Location>(input);



            await _locationRepository.InsertAsync(location);

        }

        [AbpAuthorize(AppPermissions.Pages_Locations_Edit)]
        protected virtual async Task Update(CreateOrEditLocationDto input)
        {
            var location = await _locationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, location);

        }

        [AbpAuthorize(AppPermissions.Pages_Locations_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _locationRepository.DeleteAsync(input.Id);
        }

    }
}