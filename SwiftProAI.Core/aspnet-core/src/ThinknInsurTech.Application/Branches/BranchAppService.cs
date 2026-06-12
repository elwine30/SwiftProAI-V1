using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Branches.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Branches
{
    [AbpAuthorize(AppPermissions.Pages_Branch)]
    public class BranchAppService : ThinknInsurTechAppServiceBase, IBranchAppService
    {
        private readonly IRepository<Branch> _branchRepository;

        public BranchAppService(IRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;

        }

        public virtual async Task<PagedResultDto<GetBranchForViewDto>> GetAll(GetAllBranchInput input)
        {

            var filteredBranch = _branchRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ShortName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortNameFilter), e => e.ShortName.Contains(input.ShortNameFilter));

            var pagedAndFilteredBranch = filteredBranch
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var branch = from o in pagedAndFilteredBranch
                         select new
                         {

                             o.Name,
                             o.ShortName,
                             Id = o.Id
                         };

            var totalCount = await filteredBranch.CountAsync();

            var dbList = await branch.ToListAsync();
            var results = new List<GetBranchForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBranchForViewDto()
                {
                    Branch = new BranchDto
                    {

                        Name = o.Name,
                        ShortName = o.ShortName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBranchForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetBranchForViewDto> GetBranchForView(int id)
        {
            var branch = await _branchRepository.GetAsync(id);

            var output = new GetBranchForViewDto { Branch = ObjectMapper.Map<BranchDto>(branch) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Branch_Edit)]
        public virtual async Task<GetBranchForEditOutput> GetBranchForEdit(EntityDto input)
        {
            var branch = await _branchRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBranchForEditOutput { Branch = ObjectMapper.Map<CreateOrEditBranchDto>(branch) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditBranchDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Branch_Create)]
        protected virtual async Task Create(CreateOrEditBranchDto input)
        {
            var branch = ObjectMapper.Map<Branch>(input);

            if (AbpSession.TenantId != null)
            {
                branch.TenantId = (int)AbpSession.TenantId;
            }
            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                branch.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            await _branchRepository.InsertAsync(branch);

        }

        [AbpAuthorize(AppPermissions.Pages_Branch_Edit)]
        protected virtual async Task Update(CreateOrEditBranchDto input)
        {
            var branch = await _branchRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, branch);

        }

        [AbpAuthorize(AppPermissions.Pages_Branch_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _branchRepository.DeleteAsync(input.Id);
        }

    }
}