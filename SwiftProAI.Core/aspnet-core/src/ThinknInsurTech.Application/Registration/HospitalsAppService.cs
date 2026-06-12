using ThinknInsurTech.Common;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;
using Abp.Domain.Uow;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_Hospitals)]
    public class HospitalsAppService : ThinknInsurTechAppServiceBase, IHospitalsAppService
    {
        private readonly IRepository<Hospital> _hospitalRepository;
        private readonly IRepository<Location, int> _lookup_locationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

            
        public HospitalsAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Hospital> hospitalRepository, 
            IRepository<Location, int> lookup_locationRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _hospitalRepository = hospitalRepository;
            _lookup_locationRepository = lookup_locationRepository;

        }

        public virtual async Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input)
        {

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var filteredHospitals = _hospitalRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LocationNameFilter), e => e.CountryLocationFk != null && e.CountryLocationFk.Name == input.LocationNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LocationName2Filter), e => e.StateLocationFk != null && e.StateLocationFk.Name == input.LocationName2Filter)
                .Select(s => new
                {
                    s.Name,
                    s.Address,
                    s.Id,
                    CountryLocation = s.CountryLocationFk.Name,
                    StateLocation = s.StateLocationFk.Name,
                });

                var pagedAndFilteredHospitals = filteredHospitals
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var totalCount = await filteredHospitals.CountAsync();

                var dbList = await filteredHospitals.ToListAsync();
                var results = new List<GetHospitalForViewDto>();

                foreach (var o in dbList)
                {

                    var res = new GetHospitalForViewDto()
                    {
                        Hospital = new HospitalDto
                        {

                            Name = o.Name,
                            Address = o.Address,
                            Id = o.Id,
                        },
                        LocationName = o.CountryLocation,
                        LocationName2 = o.StateLocation 
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetHospitalForViewDto>(
                    totalCount,
                    results
                );

            }



        }

        public virtual async Task<GetHospitalForViewDto> GetHospitalForView(int id)
        {
            var hospital = await _hospitalRepository.GetAsync(id);

            var output = new GetHospitalForViewDto { Hospital = ObjectMapper.Map<HospitalDto>(hospital) };

            if (output.Hospital.CountryLocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Hospital.CountryLocationId);
                output.LocationName = _lookupLocation?.Name?.ToString();
            }

            if (output.Hospital.StateLocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Hospital.StateLocationId);
                output.LocationName2 = _lookupLocation?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Hospitals_Edit)]
        public virtual async Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto input)
        {
            var hospital = await _hospitalRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHospitalForEditOutput { Hospital = ObjectMapper.Map<CreateOrEditHospitalDto>(hospital) };

            if (output.Hospital.CountryLocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Hospital.CountryLocationId);
                output.LocationName = _lookupLocation?.Name?.ToString();
            }

            if (output.Hospital.StateLocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Hospital.StateLocationId);
                output.LocationName2 = _lookupLocation?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditHospitalDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Hospitals_Create)]
        protected virtual async Task Create(CreateOrEditHospitalDto input)
        {
            var hospital = ObjectMapper.Map<Hospital>(input);

            await _hospitalRepository.InsertAsync(hospital);

        }

        [AbpAuthorize(AppPermissions.Pages_Hospitals_Edit)]
        protected virtual async Task Update(CreateOrEditHospitalDto input)
        {
            var hospital = await _hospitalRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, hospital);

        }

        [AbpAuthorize(AppPermissions.Pages_Hospitals_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _hospitalRepository.DeleteAsync(input.Id);
        }





    }
}