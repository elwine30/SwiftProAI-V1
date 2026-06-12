using ThinknInsurTech.Branches;

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
using ThinknInsurTech.Registration;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Groups)]
    public class GroupsAppService : ThinknInsurTechAppServiceBase, IGroupsAppService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Branch, int> _lookup_branchRepository;

        public GroupsAppService(IRepository<Group> groupRepository, IRepository<Branch, int> lookup_branchRepository)
        {
            _groupRepository = groupRepository;
            _lookup_branchRepository = lookup_branchRepository;

        }

        public virtual async Task<PagedResultDto<GetGroupForViewDto>> GetAll(GetAllGroupsInput input)
        {

            var filteredGroups = _groupRepository.GetAll()
                        .Include(e => e.BranchFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.GroupTypeFilter.HasValue && input.GroupTypeFilter > -1, e => e.GroupType == input.GroupTypeFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNameFilter), e => e.BranchFk != null && e.BranchFk.Name == input.BranchNameFilter);

            var pagedAndFilteredGroups = filteredGroups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var groups = from o in pagedAndFilteredGroups
                         join o1 in _lookup_branchRepository.GetAll() on o.BranchId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new
                         {

                             o.Name,
                             o.GroupType,
                             o.IsActive,
                             Id = o.Id,
                             BranchName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         };

            var totalCount = await filteredGroups.CountAsync();

            var dbList = await groups.ToListAsync();
            var results = new List<GetGroupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetGroupForViewDto()
                {
                    Group = new GroupDto
                    {

                        Name = o.Name,
                        GroupType = o.GroupType,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    },
                    BranchName = o.BranchName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetGroupForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetGroupForViewDto> GetGroupForView(int id)
        {
            var group = await _groupRepository.GetAsync(id);

            var output = new GetGroupForViewDto { Group = ObjectMapper.Map<GroupDto>(group) };

            if (output.Group.BranchId != null)
            {
                var _lookupBranch = await _lookup_branchRepository.FirstOrDefaultAsync((int)output.Group.BranchId);
                output.BranchName = _lookupBranch?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Groups_Edit)]
        public virtual async Task<GetGroupForEditOutput> GetGroupForEdit(EntityDto input)
        {
            var group = await _groupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGroupForEditOutput { Group = ObjectMapper.Map<CreateOrEditGroupDto>(group) };

            if (output.Group.BranchId != null)
            {
                var _lookupBranch = await _lookup_branchRepository.FirstOrDefaultAsync((int)output.Group.BranchId);
                output.BranchName = _lookupBranch?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditGroupDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Groups_Create)]
        protected virtual async Task Create(CreateOrEditGroupDto input)
        {
            var group = ObjectMapper.Map<Group>(input);



            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                group.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }
            if (AbpSession.TenantId != null)
            {
                group.TenantId = (int)AbpSession.TenantId;
            }

            await _groupRepository.InsertAsync(group);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Groups_Edit)]
        protected virtual async Task Update(CreateOrEditGroupDto input)
        {
            var group = await _groupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, group);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Groups_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _groupRepository.DeleteAsync(input.Id);
        }

    }
}