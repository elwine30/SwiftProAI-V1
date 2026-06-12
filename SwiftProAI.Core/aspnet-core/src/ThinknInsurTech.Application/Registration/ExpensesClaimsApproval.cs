using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ExpensesClaimsApproval)]
    public class ExpensesClaimsApproval : ThinknInsurTechAppServiceBase, IExpensesClaimsApproval
    {
        private readonly IRepository<CaseExpense> _caseExpenseRepository;
        private readonly IRepository<Lookup, int> _lookup_lookupRepository;
        private readonly IRepository<MainRegistration, int> _lookupRegistration;
        private readonly IRepository<User, long> _lookupUser;
        private readonly IRepository<CaseClaim> _caseClaimsRepository;
        private readonly IRepository<Staff> _staffRepository;

        public ExpensesClaimsApproval(IRepository<CaseExpense> caseExpenseRepository, IRepository<Lookup, int> lookupRepository, IRepository<MainRegistration, int> lookupRegistration, IRepository<User, long> lookupUser, IRepository<CaseClaim> caseClaimsRepository, IRepository<Staff> staffRepository)
        {
            _caseExpenseRepository = caseExpenseRepository;
            _lookup_lookupRepository = lookupRepository;
            _lookupRegistration = lookupRegistration;
            _lookupUser = lookupUser;
            _caseClaimsRepository = caseClaimsRepository;
            _staffRepository = staffRepository;
        }

        public async Task<PagedResultDto<GetExpensesApprovalForViewDTO>> GetAllExpensesApproval(GetExpensesClaimsApprovalInput input)
        {
            //var defaultDate = ParseStringToDate(input);

            List<long> groupStaffUserIds = null;

            //get group user list
            if (input.SelectedGroupFilter > 0)
            {
                groupStaffUserIds = _staffRepository.GetAll()
                            .Include(o => o.UserFk)
                            .Where(o => o.GroupId.Equals(input.SelectedGroupFilter))
                            .Select(o => o.UserId)
                            .ToList();
            }

            var query = _caseExpenseRepository.GetAll()
                //.Include(e => e.AdjusterFk)
                //.Include(e => e.RegisterFk)
                .WhereIf(input.DateFromFilter.HasValue, t => t.CreationTime >= input.DateFromFilter.Value)
                .WhereIf(input.DateToFilter.HasValue, t => t.CreationTime <= input.DateToFilter.Value)
                .WhereIf(!string.IsNullOrEmpty(input.SelectedStatusFilter), e => e.StatusFk.Code.Equals(input.SelectedStatusFilter))
                .WhereIf(groupStaffUserIds != null, e => groupStaffUserIds.Contains(e.AdjusterId))
                .WhereIf(!input.SelectedAdjusterFilter.Equals(0), e => e.AdjusterId.Equals(input.SelectedAdjusterFilter))
                .Select(e => new
                {
                    e.Id,
                    CaseNo = e.RegisterFk.CaseNo,
                    VehicleNo = e.RegisterFk.VehicleNo,
                    Adjuster = e.AdjusterFk.UserName,
                    e.RegisterId,
                    Status = e.StatusFk.Description,
                    e.TypeId,
                    e.Amount,
                    e.Remark,
                    e.Approved,
                    e.Rejected,
                    e.CreationTime
                });

            var pagedAndFilteredExpensesApproval = query
                .OrderBy(input.Sorting ?? "creationTime asc")
                .PageBy(input);

            var expenses = from o in pagedAndFilteredExpensesApproval
                           join o1 in _lookupRegistration.GetAll() on o.RegisterId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o4 in _lookup_lookupRepository.GetAll() on o.TypeId equals o4.Id into j4
                           from s4 in j4.DefaultIfEmpty()

                           select new
                           {
                               o.Id,
                               o.RegisterId,
                               CaseNo = s1 == null || s1.CaseNo == null ? "" : s1.CaseNo.ToString(),
                               VehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                               o.Adjuster,
                               o.Amount,
                               o.Remark,
                               o.Status,
                               Type = "Expensess",
                               o.Approved,
                               o.Rejected,
                               Prefix = s1 == null || s1.Prefix == null ? "" : s1.Prefix.ToString(),
                           };

            var totalCount = await query.CountAsync();
            var dbList = await expenses.ToListAsync();
            var result = new List<GetExpensesApprovalForViewDTO>();

            foreach (var o in dbList)
            {
                var res = new GetExpensesApprovalForViewDTO()
                {
                    Id = o.Id,
                    RegisterId = o.RegisterId,
                    CaseNo = o.Prefix + "" + o.CaseNo,
                    VehicleNo = o.VehicleNo,
                    Adjuster = o.Adjuster,
                    Amount = o.Amount,
                    Remark = o.Remark,
                    Status = o.Status,
                    Type = o.Type,
                    Approved = o.Approved,
                    Rejected = o.Rejected,
                };

                result.Add(res);
            }

            return new PagedResultDto<GetExpensesApprovalForViewDTO>(
                totalCount,
                result
            );
        }

        public async Task<PagedResultDto<GetClaimsApprovalForViewDTO>> GetAllClaimsForApproval(GetExpensesClaimsApprovalInput input)
        {
            //var defaultDate = ParseStringToDate(input);

            List<long> groupStaffUserIds = null;

            //get group user list
            if (input.SelectedGroupFilter > 0)
            {
                groupStaffUserIds = _staffRepository.GetAll()
                            .Include(o => o.UserFk)
                            .Where(o => o.GroupId.Equals(input.SelectedGroupFilter))
                            .Select(o => o.UserId)
                            .ToList();
            }

            var query = _caseClaimsRepository.GetAll()

                .WhereIf(input.DateFromFilter.HasValue, t => t.CreationTime >= input.DateFromFilter.Value)
                .WhereIf(input.DateToFilter.HasValue, t => t.CreationTime <= input.DateToFilter.Value)
                .WhereIf(!string.IsNullOrEmpty(input.SelectedStatusFilter), e => e.StatusFk.Code.Equals(input.SelectedStatusFilter))
                .WhereIf(groupStaffUserIds != null, e => groupStaffUserIds.Contains((long)e.CreatorUserId))
                .WhereIf(!input.SelectedAdjusterFilter.Equals(0), e => e.CreatorUserId.Equals(input.SelectedAdjusterFilter));

            var pagedAndFilteredClaimsApproval = query
                .OrderBy(input.Sorting ?? "creationTime asc")
                .PageBy(input);

            var expenses = from o in pagedAndFilteredClaimsApproval
                           join o1 in _lookupRegistration.GetAll() on o.RegisterId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_lookupRepository.GetAll() on o.StatusId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookupUser.GetAll() on o.CreatorUserId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           select new
                           {
                               o.Id,
                               CaseNo = s1 == null || s1.CaseNo == null ? "" : s1.CaseNo.ToString(),
                               VehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                               Adjuster = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                               Amount = o.Total,
                               Remark = o.FileChargesRemark,
                               Status = s2 == null || s2.Description == null ? "" : s2.Description.ToString(),
                               Type = "Claims",
                               o.Approved,
                               o.Rejected,
                               Prefix = s1 == null || s1.Prefix == null ? "" : s1.Prefix.ToString(),
                           };

            var totalCount = await query.CountAsync();
            var dbList = await expenses.ToListAsync();
            var result = new List<GetClaimsApprovalForViewDTO>();

            foreach (var o in dbList)
            {
                var res = new GetClaimsApprovalForViewDTO()
                {
                    Id = o.Id,
                    CaseNo = o.Prefix + "" + o.CaseNo,
                    VehicleNo = o.VehicleNo,
                    Adjuster = o.Adjuster,
                    Amount = o.Amount,
                    Remark = o.Remark,
                    Status = o.Status,
                    Type = o.Type,
                    Approved = o.Approved,
                    Rejected = o.Rejected,
                };

                result.Add(res);
            }

            return new PagedResultDto<GetClaimsApprovalForViewDTO>(
                totalCount,
                result
            );
        }

        public async Task UpdateExpenses(List<ExpensesClaimsApprovalDto> dtoList)
        {
            List<int> ids = dtoList.Select(x => x.Id).ToList();

            var existingEntities = await _caseExpenseRepository.GetAll()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();

            foreach (var entity in existingEntities)
            {
                var dto = dtoList.FirstOrDefault(dto => dto.Id == entity.Id);
                if (dto != null)
                {
                    entity.Approved = dto.Approved;
                    entity.Rejected = dto.Rejected;
                    
                    if(dto.Approved)
                    {
                        entity.StatusId = GetExpensesStatusIdStep(entity.StatusId);
                    }

                    if(dto.Rejected)
                    {
                        entity.StatusId = (int)EnumExpensesStatus.EXP005_REJECTED;
                    }
                }
            }

            await _caseExpenseRepository.GetDbContext().SaveChangesAsync();
        }

        public async Task UpdateClaims(List<ExpensesClaimsApprovalDto> dtoList)
        {
            List<int> ids = dtoList.Select(x => x.Id).ToList();

            var existingEntities = await _caseClaimsRepository.GetAll()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();

            foreach (var entity in existingEntities)
            {
                var dto = dtoList.FirstOrDefault(dto => dto.Id == entity.Id);
                if (dto != null)
                {
                    entity.Approved = dto.Approved;
                    entity.Rejected = dto.Rejected;

                    if (dto.Approved)
                    {
                        entity.StatusId = GetExpensesStatusIdStep(entity.StatusId.Value);
                    }

                    if (dto.Rejected)
                    {
                        entity.StatusId = (int)EnumExpensesStatus.EXP005_REJECTED;
                    }
                }
            }

            await _caseClaimsRepository.GetDbContext().SaveChangesAsync();
        }

        //private static ApprovalDefaultDateRange ParseStringToDate(GetExpensesClaimsApprovalInput input)
        //{

        //    DateTime startDate;
        //    DateTime endDate;

        //    string dateFromFilter = input.DateFromFilter;
        //    string dateToFilter = input.DateToFilter;

        //    switch (!string.IsNullOrEmpty(dateFromFilter))
        //    {
        //        case true:
        //            startDate = DateTime.ParseExact(dateFromFilter, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            break;
        //        case false:
        //            DateTime today = DateTime.Today;
        //            startDate = new DateTime(today.Year, today.Month, 1); // First day of the current month
        //            break;
        //    }

        //    switch (!string.IsNullOrEmpty(dateToFilter))
        //    {
        //        case true:
        //            endDate = DateTime.ParseExact(dateToFilter, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date.AddDays(1).AddTicks(-1);
        //            break;
        //        case false:
        //            endDate = DateTime.Today.Date.AddDays(1).AddTicks(-1); // End of today (23:59:59)
        //            break;
        //    }

        //    return new ApprovalDefaultDateRange()
        //    {
        //        StartDate = startDate,
        //        EndDate = endDate
        //    };
        //}

        private static int GetExpensesStatusIdStep(int statusId)
        {
            switch (statusId)
            {
                case (int)EnumExpensesStatus.EXP001_PENDING_FOR_APPROVAL:
                    statusId = (int)EnumExpensesStatus.EXP002_PENDING_FOR_PAYMENT;
                    break;
                case (int)EnumExpensesStatus.EXP002_PENDING_FOR_PAYMENT:
                    statusId = (int)EnumExpensesStatus.EXP003_PAYMENT_DONE;
                    break;
                default:
                    break;
            }

            return statusId;
        }
    }
}
