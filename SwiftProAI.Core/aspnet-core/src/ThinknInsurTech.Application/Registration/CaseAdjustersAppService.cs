using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Dependency;
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
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Case;
using ThinknInsurTech.Common;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseAdjusters)]
    public class CaseAdjustersAppService : ThinknInsurTechAppServiceBase, ICaseAdjustersAppService, ITransientDependency
    {
        private readonly IRepository<CaseAdjuster> _caseAdjusterRepository;
        private readonly IRepository<ScopeAssignment, int> _lookup_scopeAssignmentRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<CaseType, int> _caseTypeRepository;
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Branch, int> _branchRepository;
        private readonly IRepository<Location, int> _lookup_locationRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;


        public CaseAdjustersAppService(
            IRepository<CaseAdjuster> caseAdjusterRepository,
            IRepository<ScopeAssignment, int> lookup_scopeAssignmentRepository,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository,
            IRepository<CaseType, int> caseTypeRepository,
            IRepository<Staff, int> staffRepository,
            IRepository<User, long> userRepository, IUnitOfWorkManager unitOfWorkManager,
            IRepository<Branch, int> branchRepository, IRepository<Location, int> lookup_locationRepository, IRepository<User, long> lookup_userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository
)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _caseAdjusterRepository = caseAdjusterRepository;
            _lookup_scopeAssignmentRepository = lookup_scopeAssignmentRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _caseTypeRepository = caseTypeRepository;
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _lookup_locationRepository = lookup_locationRepository;
            _lookup_userRepository = lookup_userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;

        }

        public virtual async Task<PagedResultDto<GetCaseAdjusterForViewDto>> GetAll(GetAllCaseAdjustersInput input)
        {

            var filteredCaseAdjusters = _caseAdjusterRepository.GetAll()
                        .Include(e => e.ScopeAssignmentFk)
                        .Include(e => e.RegisterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Status.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status.Contains(input.StatusFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ScopeAssignmentDescriptionFilter), e => e.ScopeAssignmentFk != null && e.ScopeAssignmentFk.Description == input.ScopeAssignmentDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MainRegistrationVehicleNoFilter), e => e.RegisterFk != null && e.RegisterFk.VehicleNo == input.MainRegistrationVehicleNoFilter);

            var pagedAndFilteredCaseAdjusters = filteredCaseAdjusters
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseAdjusters = from o in pagedAndFilteredCaseAdjusters
                                join o1 in _lookup_scopeAssignmentRepository.GetAll() on o.ScopeAssignmentId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.Status,
                                    o.Id,
                                    o.RegisterId,
                                    ScopeAssignmentDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                    RegistrationCaseTypeId = s2 == null ? "" : s2.CaseTypeId.ToString(),
                                    AdjusterName = s2 == null ? "" : s2.AdjusterMemberId.ToString(),
                                    AdjusterContact = s2 == null ? "" : s2.AdjusterMemberId.ToString(),
                                    AssignmentTime = s2.CreationTime,
                                    CompletionTime = s2.CreationTime.AddDays(14),
                                    ExtendedCompletionDate = s2.ExtendedCompletionDate,
                                    ExtendCompletionRemark = s2.ExtendCompletionRemark,
                                };

            var totalCount = await filteredCaseAdjusters.CountAsync();

            var dbList = await caseAdjusters.ToListAsync();
            var results = new List<GetCaseAdjusterForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseAdjusterForViewDto()
                {
                    CaseAdjuster = new CaseAdjusterDto
                    {

                        Status = o.Status,
                        Id = o.Id,
                        RegisterId = o.RegisterId,
                        ExtendedCompletionDate = o.ExtendedCompletionDate,
                        ExtendCompletionRemark = o.ExtendCompletionRemark,
                    },
                    ScopeAssignmentDescription = o.ScopeAssignmentDescription,
                    RegistrationCaseTypeId = o.RegistrationCaseTypeId,
                    AdjusterName = o.AdjusterName,
                    AdjusterContact = o.AdjusterContact,
                    AssignmentTime = o.AssignmentTime,
                    CompletionTime = o.CompletionTime,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseAdjusterForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCaseAdjusterForViewDto> GetCaseAdjusterForView(int registerId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {


                var currentOUId = AbpSession.GetCurrentOUId().Value;

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == registerId).Select(f => f.RegisterId).FirstOrDefault();

                var caseAdjuster = _caseAdjusterRepository.GetAll().Where(w => w.RegisterId.Equals(registerId)).FirstOrDefault();
                var output = new GetCaseAdjusterForViewDto { CaseAdjuster = caseAdjuster != null ? ObjectMapper.Map<CaseAdjusterDto>(caseAdjuster) : new CaseAdjusterDto() };

                if (caseAdjuster != null && caseAdjuster.ScopeAssignmentId.HasValue)
                {
                    var _lookupScopeAssignment = await _lookup_scopeAssignmentRepository.FirstOrDefaultAsync((int)output.CaseAdjuster.ScopeAssignmentId);
                    output.ScopeAssignmentDescription = _lookupScopeAssignment?.Description?.ToString();
                }
                if (caseAdjuster != null && caseAdjuster.StateLocationId.HasValue)
                {
                    var _lookupState = await _lookup_locationRepository.FirstOrDefaultAsync((int)caseAdjuster.StateLocationId);
                    output.StateName = _lookupState?.Name?.ToString();
                }
                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(registerId);

                if (_lookupMainRegistration != null)
                {
                    if (_lookupMainRegistration.CaseTypeId != null && _lookupMainRegistration.AdjusterMemberId > 0)
                    {
                        output.CaseTypeName = _caseTypeRepository.Get(_lookupMainRegistration.CaseTypeId).ShortName;
                    }

                    if (_lookupMainRegistration.AdjusterMemberId != null && _lookupMainRegistration.AdjusterMemberId > 0)
                    {
                        var adjuster = await _userRepository.FirstOrDefaultAsync(_lookupMainRegistration.AdjusterMemberId);
                        output.AdjusterName = adjuster?.Name ?? string.Empty;
                        output.AdjusterContact = adjuster?.PhoneNumber ?? string.Empty;
                    }

                    if (_lookupMainRegistration.BranchId != null && _lookupMainRegistration.BranchId > 0)
                    {
                        output.BranchName = _lookup_locationRepository.Get(_lookupMainRegistration.BranchId).Name;

                    }
                    if (_lookupMainRegistration.EditorMemberId != null && _lookupMainRegistration.EditorMemberId > 0)
                    {
                        output.EditorUserName = _userRepository.Get((long)_lookupMainRegistration.EditorMemberId).Name;
                    }

                    output.AssignmentTime = _lookupMainRegistration.AssignTime;
                    output.CompletionTime = _lookupMainRegistration.AssignTime.AddDays(14);

                    output.CaseAdjuster.ExtendedCompletionDate = _lookupMainRegistration?.ExtendedCompletionDate;
                    output.CaseAdjuster.ExtendCompletionRemark = _lookupMainRegistration?.ExtendCompletionRemark;
                }

                return output;

            }


        }


        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters_Edit)]
        public virtual async Task<GetCaseAdjusterForEditOutput> GetCaseAdjusterForEdit(EntityDto input)
        {
            var caseAdjuster = _caseAdjusterRepository.GetAll().Where(w => w.RegisterId.Equals(input.Id)).FirstOrDefault();

            var output = new GetCaseAdjusterForEditOutput { CaseAdjuster = caseAdjuster != null ? ObjectMapper.Map<CreateOrEditCaseAdjusterDto>(caseAdjuster) : new CreateOrEditCaseAdjusterDto() };

            if (caseAdjuster != null && caseAdjuster.ScopeAssignmentId.HasValue)
            {
                var _lookupScopeAssignment = await _lookup_scopeAssignmentRepository.FirstOrDefaultAsync((int)output.CaseAdjuster.ScopeAssignmentId);
                output.ScopeAssignmentDescription = _lookupScopeAssignment?.Description?.ToString();
            }
            var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);

            if (_lookupMainRegistration != null)
            {
                output.CaseTypeId = _lookupMainRegistration.CaseTypeId;
                output.BranchId = _lookupMainRegistration.BranchId;
                output.AdjusterMemberId = _lookupMainRegistration.AdjusterMemberId;
                output.AdjusterContact = _userRepository.FirstOrDefault(output.AdjusterMemberId).PhoneNumber;
                output.AssignmentTime = _lookupMainRegistration.AssignTime;
                output.CompletionTime = _lookupMainRegistration.AssignTime.AddDays(14);
                output.CaseAdjuster.ExtendedCompletionDate = _lookupMainRegistration?.ExtendedCompletionDate;
                output.CaseAdjuster.ExtendCompletionRemark = _lookupMainRegistration?.ExtendCompletionRemark;
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseAdjusterDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters_Create)]
        protected virtual async Task Create(CreateOrEditCaseAdjusterDto input)
        {
            var caseAdjuster = ObjectMapper.Map<CaseAdjuster>(input);
            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseAdjuster.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseAdjuster.TenantId = AbpSession.TenantId.Value;
            }

            await _caseAdjusterRepository.InsertAsync(caseAdjuster);
            if (caseAdjuster != null)
            {
                var mr = _lookup_mainRegistrationRepository.Get(input.RegisterId);

                if (!string.IsNullOrEmpty(caseAdjuster.Status) && Int32.Parse(caseAdjuster.Status) == 5)
                {
                    mr.StatusId = Int32.Parse(caseAdjuster.Status);
                }

                mr.ExtendedCompletionDate = input.ExtendedCompletionDate;
                mr.ExtendCompletionRemark = input.ExtendCompletionRemark;

                _lookup_mainRegistrationRepository.Update(mr);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters_Edit)]
        protected virtual async Task Update(CreateOrEditCaseAdjusterDto input)
        {
            var caseAdjuster = await _caseAdjusterRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseAdjuster);
            if (caseAdjuster != null)
            {
                var mr = _lookup_mainRegistrationRepository.Get(input.RegisterId);

                if (!string.IsNullOrEmpty(caseAdjuster.Status) && Int32.Parse(caseAdjuster.Status) == 5)
                {
                    mr.StatusId = Int32.Parse(caseAdjuster.Status);
                }

                mr.ExtendedCompletionDate = input.ExtendedCompletionDate;
                mr.ExtendCompletionRemark = input.ExtendCompletionRemark;

                _lookup_mainRegistrationRepository.Update(mr);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters)]
        public async Task<List<CaseAdjusterScopeAssignmentLookupTableDto>> GetAllScopeAssignmentForTableDropdown()
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                return await _lookup_scopeAssignmentRepository.GetAll()
                .Select(scopeAssignment => new CaseAdjusterScopeAssignmentLookupTableDto
                {
                    Id = scopeAssignment.Id,
                    DisplayName = scopeAssignment == null || scopeAssignment.Description == null ? "" : scopeAssignment.Description.ToString()
                })
                //Requirements - place others at the last for better UI
                .OrderBy(scopeAssignment => scopeAssignment.DisplayName == "others").ThenBy(scopeAssignment => scopeAssignment.DisplayName)
                .ToListAsync();
            }
        }



        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters)]
        public async Task<List<CaseAdjusterUserLookupTableDto>> GetAllEditorUserForTableDropdown()
        {
            IQueryable<CaseAdjusterUserLookupTableDto> query = _userRepository.GetAll()
                .Join(_userRoleRepository.GetAll(), user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
                .Join(_roleRepository.GetAll(), joined => joined.userRole.RoleId, role => role.Id, (joined, role) => new { joined.user, joined.userRole, role })
                .Where(j => j.role.Name == StaticRoleNames.Tenants.Editor)
                .OrderBy(j => j.user.Name)
                .Select(j => new CaseAdjusterUserLookupTableDto
                {
                    Id = j.user.Id,
                    DisplayName = j.user.Name + " (" + j.user.UserName + ")",
                });

            var editors = await query.ToListAsync();
            return editors;
        }

        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters)]
        public async Task<List<CaseAdjusterLookupTableDto>> GetAllCaseTypeForTableDropdown()
        {
            return await _caseTypeRepository.GetAll()
                .Select(caseType => new CaseAdjusterLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.ShortName == null ? "" : caseType.ShortName.ToString()
                }).ToListAsync();
        }


        [AbpAuthorize(AppPermissions.Pages_CaseAdjusters)]
        public async Task<List<CaseAdjusterLocationLookupTableDto>> GetAllStateLocationForTableDropdown(int parentId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

                var location = await _lookup_locationRepository.GetAll().Where(w => w.ParentLocationId == parentId)
                .OrderBy(x => x.Name)
                .Select(location => new CaseAdjusterLocationLookupTableDto
                {
                    Id = location.Id,
                    DisplayName = location == null || location.Name == null ? "" : location.Name.ToString()
                }).ToListAsync();

                return location;

            }
        }

    }
}