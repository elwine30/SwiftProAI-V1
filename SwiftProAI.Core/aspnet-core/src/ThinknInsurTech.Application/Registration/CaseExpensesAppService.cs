using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseExpenses)]
    public class CaseExpensesAppService : ThinknInsurTechAppServiceBase, ICaseExpensesAppService
    {
        private readonly IRepository<CaseExpense> _caseExpenseRepository;
        private readonly IRepository<Lookup, int> _lookup_lookupRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public CaseExpensesAppService(IRepository<CaseExpense> caseExpenseRepository, IRepository<Lookup, int> lookup_lookupRepository, IRepository<User, long> lookup_userRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _caseExpenseRepository = caseExpenseRepository;
            _lookup_lookupRepository = lookup_lookupRepository;
            _lookup_userRepository = lookup_userRepository;
            _unitOfWorkManager = unitOfWorkManager;


        }

        public virtual async Task<PagedResultDto<GetCaseExpenseForViewDto>> GetAll(GetAllCaseExpensesInput input)
        {

            var filteredCaseExpenses = _caseExpenseRepository.GetAll()
                        .Include(e => e.StatusFk)
                        .Include(e => e.TypeFk)
                        .Include(e => e.SubTypeFk)
                        .Include(e => e.RegisterFk)
                        .Include(e => e.AdjusterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remark.Contains(input.Filter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinApprovedAmountFilter != null, e => e.ApprovedAmount >= input.MinApprovedAmountFilter)
                        .WhereIf(input.MaxApprovedAmountFilter != null, e => e.ApprovedAmount <= input.MaxApprovedAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarkFilter), e => e.Remark.Contains(input.RemarkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LookupDescriptionFilter), e => e.StatusFk != null && e.StatusFk.Description == input.LookupDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LookupDescription2Filter), e => e.TypeFk != null && e.TypeFk.Description == input.LookupDescription2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LookupDescription3Filter), e => e.SubTypeFk != null && e.SubTypeFk.Description == input.LookupDescription3Filter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.LookupDescription4Filter), e => e.RegisterFk != null && e.RegisterFk.Description == input.LookupDescription4Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.AdjusterFk != null && e.AdjusterFk.Name == input.UserNameFilter);

            var pagedAndFilteredCaseExpenses = filteredCaseExpenses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseExpenses = from o in pagedAndFilteredCaseExpenses
                               join o1 in _lookup_lookupRepository.GetAll() on o.StatusId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_lookupRepository.GetAll() on o.TypeId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               join o3 in _lookup_lookupRepository.GetAll() on o.SubTypeId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               join o4 in _lookup_lookupRepository.GetAll() on o.RegisterId equals o4.Id into j4
                               from s4 in j4.DefaultIfEmpty()

                               join o5 in _lookup_userRepository.GetAll() on o.AdjusterId equals o5.Id into j5
                               from s5 in j5.DefaultIfEmpty()

                               select new
                               {

                                   o.Amount,
                                   o.ApprovedAmount,
                                   o.Remark,
                                   Id = o.Id,
                                   LookupDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                   LookupDescription2 = s2 == null || s2.Description == null ? "" : s2.Description.ToString(),
                                   LookupDescription3 = s3 == null || s3.Description == null ? "" : s3.Description.ToString(),
                                   LookupDescription4 = s4 == null || s4.Description == null ? "" : s4.Description.ToString(),
                                   UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                               };

            var totalCount = await filteredCaseExpenses.CountAsync();

            var dbList = await caseExpenses.ToListAsync();
            var results = new List<GetCaseExpenseForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseExpenseForViewDto()
                {
                    CaseExpense = new CaseExpenseDto
                    {

                        Amount = o.Amount,
                        ApprovedAmount = o.ApprovedAmount,
                        Remark = o.Remark,
                        Id = o.Id,
                    },
                    LookupDescription = o.LookupDescription,
                    LookupDescription2 = o.LookupDescription2,
                    LookupDescription3 = o.LookupDescription3,
                    LookupDescription4 = o.LookupDescription4,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseExpenseForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetCaseExpenseForViewDto> GetCaseExpenseForView(int id)
        {
            var caseExpense = await _caseExpenseRepository.GetAsync(id);

            var output = new GetCaseExpenseForViewDto { CaseExpense = ObjectMapper.Map<CaseExpenseDto>(caseExpense) };

            if (output.CaseExpense.StatusId != null)
            {
                var _lookupLookup = await _lookup_lookupRepository.FirstOrDefaultAsync((int)output.CaseExpense.StatusId);
                output.LookupDescription = _lookupLookup?.Description?.ToString();
            }

            if (output.CaseExpense.TypeId != null)
            {
                var _lookupLookup = await _lookup_lookupRepository.FirstOrDefaultAsync((int)output.CaseExpense.TypeId);
                output.LookupDescription2 = _lookupLookup?.Description?.ToString();
            }

            if (output.CaseExpense.SubTypeId != null)
            {
                var _lookupLookup = await _lookup_lookupRepository.FirstOrDefaultAsync((int)output.CaseExpense.SubTypeId);
                output.LookupDescription3 = _lookupLookup?.Description?.ToString();
            }

            if (output.CaseExpense.RegisterId != null)
            {
                var _lookupLookup = await _lookup_lookupRepository.FirstOrDefaultAsync((int)output.CaseExpense.RegisterId);
                output.LookupDescription4 = _lookupLookup?.Description?.ToString();
            }

            if (output.CaseExpense.AdjusterId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseExpense.AdjusterId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }


        [AbpAuthorize(AppPermissions.Pages_CaseExpenses_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseExpenseRepository.DeleteAsync(input.Id);
        }

        #region Adjuster Usage
        public async Task<GetCaseExpenseAdjusterViewDto> CreateExpenses(CreateExpenseInput input)
        {
            try
            {
                var currentUser = AbpSession.ToUserIdentifier();

                long? organizationUnitId = null;

                // Check if the user belongs to any organization unit
                if (AbpSession.GetCurrentOUId() != null)
                {
                    organizationUnitId = AbpSession.GetCurrentOUId().Value;
                }

                var newExpenses = new CaseExpense
                {
                    Remark = input.Remark,
                    Amount = input.Amount,
                    TypeId = input.TypeId,
                    SubTypeId = input.SubTypeId == 0 ? (int?)null : input.SubTypeId,
                    RegisterId = input.MainRegistrationId,
                    StatusId = (int)EnumExpensesStatus.EXP001_PENDING_FOR_APPROVAL,
                    ApprovedAmount = 0.00,
                    AdjusterId = (int)currentUser.UserId,
                    TenantId = (int)currentUser.TenantId,
                    OrganizationUnitId = organizationUnitId
                };

                GetCaseExpenseAdjusterViewDto dto = null;
                var expenseId = await _caseExpenseRepository.InsertAndGetIdAsync(newExpenses);

                if (expenseId > 0)
                {
                    CaseExpense ce = await _caseExpenseRepository.FirstOrDefaultAsync(x => x.Id == expenseId);
                    if (ce != null)
                    {
                        Lookup lookupType = null;
                        User user = null;

                        using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                        {
                            lookupType = _lookup_lookupRepository.FirstOrDefault((int)ce.TypeId);
                            user = _lookup_userRepository.FirstOrDefault((int)ce.CreatorUserId);
                        }

                        var typeOfExpense = lookupType?.Description ?? "N/A";
                        var createdBy = user?.UserName ?? "N/A";

                        dto = new GetCaseExpenseAdjusterViewDto()
                        {
                            Id = ce.Id,
                            Amount = ce.Amount,
                            Remarks = ce.Remark,
                            TypeOfExpenses = typeOfExpense,
                            CreatedBy = createdBy,
                            CreatedDate = ce.CreationTime,
                        };
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                Logger.Error("Error creating expense", ex);
                throw;
            }
        }

        public virtual async Task UpdateCaseExpenses(UpdateCaseExpensesDTO input)
        {
            var id = input.Id;

            try
            {
                var caseExpenses = await _caseExpenseRepository.FirstOrDefaultAsync((int)input.Id);

                if (caseExpenses != null)
                {
                    caseExpenses.Amount = input.Amount;
                    caseExpenses.Remark = input.Remark;
                    caseExpenses.TypeId = input.TypeId;
                }

                _caseExpenseRepository.Update(caseExpenses);
            }
            catch (Exception ex)
            {
                Logger.Error("Error creating expense", ex);
                throw;
            }
        }


        public async Task<List<GetCaseExpenseAdjusterViewDto>> GetCaseExpenseAdjusterViewDto(int id)
        {
            var caseExpenses = await _caseExpenseRepository.GetAll()
                .Where(mr => mr.RegisterId == id)
                .ToListAsync();

            var caseExpensesDetails = new List<GetCaseExpenseAdjusterViewDto>();

            foreach (var ce in caseExpenses)
            {
                Lookup lookupType = null;
                User user = null;

                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    lookupType = _lookup_lookupRepository.FirstOrDefault((int)ce.TypeId);
                    user = _lookup_userRepository.FirstOrDefault((int)ce.CreatorUserId);
                }

                var typeOfExpense = lookupType?.Description ?? "N/A";
                var createdBy = user?.UserName ?? "N/A";

                caseExpensesDetails.Add(new GetCaseExpenseAdjusterViewDto()
                {
                    Id = ce.Id,
                    Amount = ce.Amount,
                    Remarks = ce.Remark,
                    TypeOfExpenses = typeOfExpense,
                    CreatedBy = createdBy,
                    CreatedDate = ce.CreationTime,
                });
            }

            return caseExpensesDetails;
        }


        [AbpAuthorize(AppPermissions.Pages_CaseExpenses)]
        public async Task<List<CaseExpenseLookupTableDto>> GetExpensesTypeList(string lookupGroup)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await _lookup_lookupRepository.GetAll()
               .Where(lookup => lookup.Group.Equals(lookupGroup))
               .Select(expType => new CaseExpenseLookupTableDto
               {
                   Id = expType.Id,
                   DisplayName = expType == null || expType.Description == null ? "" : expType.Description.ToString()
               }).ToListAsync();
            }
        }
        #endregion


    }
}