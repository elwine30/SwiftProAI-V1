using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Reports.Dto;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Reports.Exporting;

namespace ThinknInsurTech.Reports
{
    [AbpAuthorize(AppPermissions.Pages_AdjusterReports)]
    public class AdjusterReportsAppService : ThinknInsurTechAppServiceBase, IAdjusterReportsAppService
    {
        private readonly IAdjusterReportsExcelExporter _adjusterReportsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainregistration;
        private readonly IRepository<CaseInsurer, int> _lookup_caseInsurer;
        private readonly IRepository<CaseInvoice, int> _lookup_invoiceRepository;


        public AdjusterReportsAppService(IRepository<MainRegistration, int> lookup_mainregistration, IRepository<CaseInvoice, int> lookup_invoiceRepository, IRepository<CaseInsurer, int> lookup_caseInsurer, IAdjusterReportsExcelExporter adjusterReportsExcelExporter, IRepository<User, long> lookup_userRepository)
        {
            _adjusterReportsExcelExporter = adjusterReportsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_mainregistration = lookup_mainregistration;
            _lookup_caseInsurer = lookup_caseInsurer;
            _lookup_invoiceRepository = lookup_invoiceRepository;
        }

        public virtual async Task<PagedResultDto<GetAdjusterReportForViewDto>> GetAll(GetAllAdjusterReportsInput input)
        {
            var filteredAdjusterReports =
             (from o in _lookup_mainregistration.GetAll()
                .Where(x => x.StatusId == (int)EnumRegistrationStatus.CompletedInvoices)
                .WhereIf(input.UserIdFilter.HasValue, o => o.AdjusterMemberId == input.UserIdFilter)
              join o1 in _lookup_caseInsurer.GetAll() on o.Id equals o1.RegisterId into j1
              from s1 in j1.DefaultIfEmpty()
              join o2 in _lookup_invoiceRepository.GetAll() on o.Id equals o2.RegisterId into j2
              from s2 in j2.DefaultIfEmpty()
              select new GetAdjusterReportForViewDto
              {
                  CreatedDate = o.CreationTime,
                  CaseId = o.Id,
                  InsuranceCompany = o.Company.ShortName,
                  CaseType = o.CaseType.ShortName,
                  VehicleNo = o.VehicleNo,
                  InsuranceCaseRef = s1 != null ? s1.ReferenceNo : "N/A",
                  ServiceFee = s2 != null ? s2.ServiceAmount : 0.00M,
                  AssignmentDate = o.AssignTime,
                  CaseNo = o.CaseNo,
              })
             .WhereIf(input.YearFilter != DateTime.MinValue && input.MonthFilter == DateTime.MinValue, e => e.AssignmentDate.Year == input.YearFilter.Year)
             .WhereIf(input.MonthFilter != DateTime.MinValue && input.YearFilter == DateTime.MinValue, e => e.AssignmentDate.Month == input.MonthFilter.Month)
             .WhereIf(input.YearFilter != DateTime.MinValue && input.MonthFilter != DateTime.MinValue, e => e.AssignmentDate.Year == input.YearFilter.Year && e.AssignmentDate.Month == input.MonthFilter.Month);

            var totalCount = await filteredAdjusterReports.CountAsync();

            var pagedAndFilteredAdjusterReports = filteredAdjusterReports
                 .OrderBy(input.Sorting ?? "CaseId asc")
                 .PageBy(input);

            var adjusterReports = await pagedAndFilteredAdjusterReports.ToListAsync();
            var results = adjusterReports;



            return new PagedResultDto<GetAdjusterReportForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_AdjusterReports)]
        public async Task<PagedResultDto<AdjusterReportUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AdjusterReportUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new AdjusterReportUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<AdjusterReportUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<FileDto> GetAdjusterReportsToExcel(GetAllAdjusterReportsForExcelInput input)
        {

            var filteredAdjusterReports = _lookup_mainregistration.GetAll()
                         //.Include(e => e.AdjusterFk)
                         .Where(x=>x.StatusId == (int)EnumRegistrationStatus.CompletedInvoices)
                         .WhereIf(input.YearFilter != DateTime.MinValue && input.MonthFilter == DateTime.MinValue, e => e.AssignTime.Year == input.YearFilter.Year)
                         .WhereIf(input.MonthFilter != DateTime.MinValue && input.YearFilter == DateTime.MinValue, e => e.AssignTime.Month == input.MonthFilter.Month)
                         .WhereIf(input.YearFilter != DateTime.MinValue && input.MonthFilter != DateTime.MinValue, e => e.AssignTime.Year == input.YearFilter.Year && e.AssignTime.Month == input.MonthFilter.Month)
                         .WhereIf(input.UserIdFilter.HasValue, e => e.AdjusterMemberId == input.UserIdFilter);

            var query = (from o in filteredAdjusterReports
                         join o1 in _lookup_caseInsurer.GetAll() on o.Id equals o1.RegisterId into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _lookup_invoiceRepository.GetAll() on o.Id equals o2.RegisterId into j2
                         from s2 in j2.DefaultIfEmpty()
                         select new GetAdjusterReportForViewDto()
                         {
                             CreatedDate = o.CreationTime,
                             CaseId = o.Id,
                             InsuranceCompany = o.Company.Name,
                             CaseType = o.CaseType.ShortName,
                             VehicleNo = o.VehicleNo,
                             InsuranceCaseRef = s1 != null ? s1.ReferenceNo : "N/A",
                             ServiceFee = s2 != null ? s2.ServiceAmount : 0.00M,
                             AssignmentDate = o.AssignTime,
                             CaseNo = o.CaseNo,
                         });

            var adjusterReportListDtos = await query.ToListAsync();

            return _adjusterReportsExcelExporter.ExportToFile(adjusterReportListDtos);
        }

    }
}