using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common.Dto;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_Lookups)]
    public class LookupsAppService : ThinknInsurTechAppServiceBase, ILookupsAppService
    {
        private readonly IRepository<Lookup> _lookupRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public LookupsAppService(IRepository<Lookup> lookupRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _lookupRepository = lookupRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task<PagedResultDto<GetLookupForViewDto>> GetAll(GetAllLookupsInput input)
        {

            var filteredLookups = _lookupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Group.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupFilter), e => e.Group.Contains(input.GroupFilter));

            var pagedAndFilteredLookups = filteredLookups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var lookups = from o in pagedAndFilteredLookups
                          select new
                          {

                              o.Code,
                              o.Description,
                              o.Active,
                              o.Sequence,
                              o.Group,
                              Id = o.Id
                          };

            var totalCount = await filteredLookups.CountAsync();

            var dbList = await lookups.ToListAsync();
            var results = new List<GetLookupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLookupForViewDto()
                {
                    Lookup = new LookupDto
                    {

                        Code = o.Code,
                        Description = o.Description,
                        Active = o.Active,
                        Sequence = o.Sequence,
                        Group = o.Group,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLookupForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetLookupForViewDto> GetLookupForView(int id)
        {
            var lookup = await _lookupRepository.GetAsync(id);

            var output = new GetLookupForViewDto { Lookup = ObjectMapper.Map<LookupDto>(lookup) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditLookupDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Lookups_Create)]
        protected virtual async Task Create(CreateOrEditLookupDto input)
        {
            var lookup = ObjectMapper.Map<Lookup>(input);

            if (AbpSession.TenantId != null)
            {
                lookup.TenantId = (int?)AbpSession.TenantId;
            }

            await _lookupRepository.InsertAsync(lookup);

        }

        [AbpAuthorize(AppPermissions.Pages_Lookups_Edit)]
        protected virtual async Task Update(CreateOrEditLookupDto input)
        {
            var lookup = await _lookupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, lookup);

        }

        [AbpAuthorize(AppPermissions.Pages_Lookups_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _lookupRepository.DeleteAsync(input.Id);
        }

        public List<GetLookupByGroupDto> GetByGroup(string group)
        {
            if (string.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            var lookups = new List<GetLookupByGroupDto>();

            var trimmedGroup = group.Trim();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                lookups = _lookupRepository.GetAll()
                    .Where(l => l.Group.Trim().Equals(trimmedGroup) && l.Active)
                    .OrderBy(l => l.Sequence)
                    .Select(l => new GetLookupByGroupDto
                    {
                        id = l.Id,
                        Code = l.Code,
                        Description = l.Description,
                        Sequence = l.Sequence,
                    }).ToList();
            }


            return lookups;
        }

        public Dictionary<string, List<GetLookupByGroupDto>> GetAllLookupsByGroups(List<string> groups)
        {
            if (groups == null || groups.Count == 0)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            var trimmedGroups = groups.Select(g => g.Trim()).ToList();
            Dictionary<string, List<GetLookupByGroupDto>> allLookups = new Dictionary<string, List<GetLookupByGroupDto>>();

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var lookups = _lookupRepository.GetAll()
                    .Where(l => trimmedGroups.Contains(l.Group.Trim()))
                    .Select(l => new GetLookupByGroupDto
                    {
                        id = l.Id,
                        Code = l.Code,
                        Description = l.Description,
                        Sequence = l.Sequence,
                        Group = l.Group                        
                    })
                    .OrderBy(x=>x.Description)
                    .ToList();

                // Group lookups by group name
                foreach (var group in trimmedGroups)
                {
                    allLookups[group] = lookups.Where(l => l.Group.Trim().Equals(group)).ToList();
                }
            }

            return allLookups;
        }


    }
}