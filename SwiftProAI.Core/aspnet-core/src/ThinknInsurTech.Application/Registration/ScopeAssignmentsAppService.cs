using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ScopeAssignments)]
    public class ScopeAssignmentsAppService : ThinknInsurTechAppServiceBase, IScopeAssignmentsAppService
    {
        private readonly IRepository<ScopeAssignment> _scopeAssignmentRepository;

        public ScopeAssignmentsAppService(IRepository<ScopeAssignment> scopeAssignmentRepository)
        {
            _scopeAssignmentRepository = scopeAssignmentRepository;

        }

        public virtual async Task<PagedResultDto<GetScopeAssignmentForViewDto>> GetAll(GetAllScopeAssignmentsInput input)
        {

            var filteredScopeAssignments = _scopeAssignmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.isActiveFilter.HasValue && input.isActiveFilter > -1, e => (input.isActiveFilter == 1 && e.isActive) || (input.isActiveFilter == 0 && !e.isActive));

            var pagedAndFilteredScopeAssignments = filteredScopeAssignments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var scopeAssignments = from o in pagedAndFilteredScopeAssignments
                                   select new
                                   {

                                       o.Description,
                                       o.isActive,
                                       Id = o.Id
                                   };

            var totalCount = await filteredScopeAssignments.CountAsync();

            var dbList = await scopeAssignments.ToListAsync();
            var results = new List<GetScopeAssignmentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetScopeAssignmentForViewDto()
                {
                    ScopeAssignment = new ScopeAssignmentDto
                    {

                        Description = o.Description,
                        isActive = o.isActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetScopeAssignmentForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetScopeAssignmentForViewDto> GetScopeAssignmentForView(int id)
        {
            var scopeAssignment = await _scopeAssignmentRepository.GetAsync(id);

            var output = new GetScopeAssignmentForViewDto { ScopeAssignment = ObjectMapper.Map<ScopeAssignmentDto>(scopeAssignment) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ScopeAssignments_Edit)]
        public virtual async Task<GetScopeAssignmentForEditOutput> GetScopeAssignmentForEdit(EntityDto input)
        {
            var scopeAssignment = await _scopeAssignmentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetScopeAssignmentForEditOutput { ScopeAssignment = ObjectMapper.Map<CreateOrEditScopeAssignmentDto>(scopeAssignment) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditScopeAssignmentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_ScopeAssignments_Create)]
        protected virtual async Task Create(CreateOrEditScopeAssignmentDto input)
        {
            if (input.Description.ToLower() == "others")
            {
                throw new UserFriendlyException("-Others- cannot be created as it is system default option");
            }
            var scopeAssignment = ObjectMapper.Map<ScopeAssignment>(input);

            if (AbpSession.TenantId != null)
            {
                scopeAssignment.TenantId = (int?)AbpSession.TenantId;
            }

            await _scopeAssignmentRepository.InsertAsync(scopeAssignment);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ScopeAssignments_Edit)]
        protected virtual async Task Update(CreateOrEditScopeAssignmentDto input)
        {
            if (input.Description.ToLower() == "others")
            {
                throw new UserFriendlyException("-Others- cannot be edited as it is system default option");
            }
            var scopeAssignment = await _scopeAssignmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, scopeAssignment);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ScopeAssignments_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            var scope = await _scopeAssignmentRepository.GetAsync(input.Id);

            if (scope.Description.ToLower() == "others")
            {
                throw new UserFriendlyException("-Others- cannot be deleted as it is system default option");
            }
            await _scopeAssignmentRepository.DeleteAsync(input.Id);
        }

    }
}