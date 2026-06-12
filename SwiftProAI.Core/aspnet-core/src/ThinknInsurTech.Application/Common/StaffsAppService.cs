using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;
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
using ThinknInsurTech.Registration;
using ThinknInsurTech.Runtime;


namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Staffs)]
    public class StaffsAppService : ThinknInsurTechAppServiceBase, IStaffsAppService
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Group, int> _lookup_groupRepository;

        public StaffsAppService(IRepository<Staff> staffRepository, IRepository<User, long> lookup_userRepository, IRepository<Group, int> lookup_groupRepository)
        {
            _staffRepository = staffRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_groupRepository = lookup_groupRepository;

        }

        public virtual async Task<PagedResultDto<GetStaffForViewDto>> GetAll(GetAllStaffsInput input)
        {

            var filteredStaffs = _staffRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.GroupFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NRIC.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Passport.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NRICFilter), e => e.NRIC.Contains(input.NRICFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PassportFilter), e => e.Passport.Contains(input.PassportFilter))
                        .WhereIf(input.MinServiceFeePercentFilter != null, e => e.ServiceFeePercent >= input.MinServiceFeePercentFilter)
                        .WhereIf(input.MaxServiceFeePercentFilter != null, e => e.ServiceFeePercent <= input.MaxServiceFeePercentFilter)
                        .WhereIf(input.MinFraudFeePercentFilter != null, e => e.FraudFeePercent >= input.MinFraudFeePercentFilter)
                        .WhereIf(input.MaxFraudFeePercentFilter != null, e => e.FraudFeePercent <= input.MaxFraudFeePercentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GroupNameFilter), e => e.GroupFk != null && e.GroupFk.Name == input.GroupNameFilter);

            var pagedAndFilteredStaffs = filteredStaffs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var staffs = from o in pagedAndFilteredStaffs
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_groupRepository.GetAll() on o.GroupId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new
                         {

                             o.NRIC,
                             o.Address,
                             o.Passport,
                             o.ServiceFeePercent,
                             o.FraudFeePercent,
                             Id = o.Id,
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             GroupName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         };

            var totalCount = await filteredStaffs.CountAsync();

            var dbList = await staffs.ToListAsync();
            var results = new List<GetStaffForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStaffForViewDto()
                {
                    Staff = new StaffDto
                    {

                        NRIC = o.NRIC,
                        Address = o.Address,
                        Passport = o.Passport,
                        ServiceFeePercent = o.ServiceFeePercent,
                        FraudFeePercent = o.FraudFeePercent,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    GroupName = o.GroupName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStaffForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetStaffForViewDto> GetStaffForView(int id)
        {
            var staff = await _staffRepository.GetAsync(id);

            var output = new GetStaffForViewDto { Staff = ObjectMapper.Map<StaffDto>(staff) };

            if (output.Staff.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Staff.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Staff.GroupId != null)
            {
                var _lookupGroup = await _lookup_groupRepository.FirstOrDefaultAsync((int)output.Staff.GroupId);
                output.GroupName = _lookupGroup?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Staffs_Edit)]
        public virtual async Task<GetStaffForEditOutput> GetStaffForEdit(EntityDto input)
        {
            var staff = _staffRepository.GetAll().Where(w => w.UserId.Equals(input.Id)).FirstOrDefault();

            var output = new GetStaffForEditOutput { Staff = ObjectMapper.Map<CreateOrEditStaffDto>(staff) };

            if (output.Staff != null)
            {
                if (output.Staff.GroupId != null)
                {
                    var _lookupGroup = await _lookup_groupRepository.FirstOrDefaultAsync((int)output.Staff.GroupId);
                    output.GroupName = _lookupGroup?.Name?.ToString();
                }
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditStaffDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Staffs_Create)]
        protected virtual async Task Create(CreateOrEditStaffDto input)
        {
            var staff = ObjectMapper.Map<Staff>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                staff.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                staff.TenantId = (int)AbpSession.TenantId;
            }

            await _staffRepository.InsertAsync(staff);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Staffs_Edit)]
        protected virtual async Task Update(CreateOrEditStaffDto input)
        {
            var staff = await _staffRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, staff);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Staffs_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _staffRepository.DeleteAsync(input.Id);
        }


        [AbpAuthorize(AppPermissions.Pages_Administration_Staffs)]
        public async Task<PagedResultDto<StaffGroupLookupTableDto>> GetAllGroupForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_groupRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var groupList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StaffGroupLookupTableDto>();
            foreach (var group in groupList)
            {
                lookupTableDtoList.Add(new StaffGroupLookupTableDto
                {
                    Id = group.Id,
                    DisplayName = group.Name?.ToString()
                });
            }

            return new PagedResultDto<StaffGroupLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<StaffGroupLookupTableDto>> GetAllGroupForTableDropdown()
        {
            return await _lookup_groupRepository.GetAll()
                .Select(group => new StaffGroupLookupTableDto
                {
                    Id = group.Id,
                    DisplayName = group == null || group.Name == null ? "" : group.Name.ToString()
                }).ToListAsync();
        }

        public async Task<List<StaffLookupTableDto>> GetAllStaffByGroupIdForTableDropdown(int groupId)
        {
            return await _staffRepository.GetAll()
                .Include(x => x.UserFk)
                .Where(x => x.GroupId.Equals(groupId))
                .Select(staffs => new StaffLookupTableDto
                {
                    Id = staffs.Id,
                    DisplayName = staffs.UserFk.Name.ToString()
                }).ToListAsync();

        }

    }
}