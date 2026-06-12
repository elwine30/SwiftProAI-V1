using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Reports.Exporting;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Companies;

namespace ThinknInsurTech.Reports
{
    [AbpAuthorize(AppPermissions.Pages_WIPReports)]
    public class WIPReportsAppService : ThinknInsurTechAppServiceBase, IWIPReportsAppService
    {
        private readonly IWIPReportsExcelExporter _wipReportsExcelExporter;
        private readonly IRepository<MainRegistration> _mainRegistrationRepository;
        private readonly IRepository<CaseInsurer> _caseInsurerRepository;
        private readonly IRepository<CaseLawyer> _caseLawyerRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Group> _lookup_groupRepository;
        private readonly IRepository<Staff> _lookup_staffRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;

        public WIPReportsAppService(IWIPReportsExcelExporter wipReportsExcelExporter, IRepository<MainRegistration> mainRegistrationRepository, IRepository<CaseInsurer> caseInsurerRepository, IRepository<CaseLawyer> caseLawyerRepository, IRepository<User, long> userRepository, IRepository<Group> groupRepository, IRepository<Staff> staffRepository, IRepository<InsuranceCompany,int> insuranceCompanyRepository)
        {
            _wipReportsExcelExporter = wipReportsExcelExporter;
            _mainRegistrationRepository = mainRegistrationRepository;
            _caseInsurerRepository = caseInsurerRepository;
            _caseLawyerRepository = caseLawyerRepository;
            _lookup_userRepository = userRepository;
            _lookup_groupRepository = groupRepository;
            _lookup_staffRepository = staffRepository;
            _lookup_insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public virtual async Task<PagedResultDto<GetWIPReportForViewDto>> GetAll(GetAllWIPReportsInput input)
        {
            var currentDate = DateTime.Now;
            var filteredWIPReports = (
                from o in _mainRegistrationRepository.GetAll()
                where o.StatusId != 4 && o.StatusId != 5  //Filter out (Completed Invoices, Cancelled) 
                join caseInsurer in _caseInsurerRepository.GetAll() on o.Id equals caseInsurer.RegisterId into j2
                from s2 in j2.DefaultIfEmpty()
                join caseLawyer in _caseLawyerRepository.GetAll() on o.Id equals caseLawyer.RegisterId into j3
                from s3 in j3.DefaultIfEmpty()
                join user in _lookup_userRepository.GetAll() on o.AdjusterMemberId equals user.Id into j4
                from s4 in j4.DefaultIfEmpty()
                join staff in _lookup_staffRepository.GetAll() on s4.Id equals staff.UserId into j5
                from s5 in j5.DefaultIfEmpty()
                join groups in _lookup_groupRepository.GetAll() on s5.GroupId equals groups.Id into j6
                from s6 in j6.DefaultIfEmpty()

                select new WIPReportDto
                {
                    Id = o.Id,
                    ReportDate = o.AssignTime,
                    VehicleNo = o.VehicleNo,
                    CaseReference = o.Id,
                    InsuranceCompanyId = o.CompanyId,
                    InsuranceCompany = o.Company.ShortName,
                    CaseType = o.CaseType.ShortName,
                    AdjusterId = s4 != null ? s4.Id : 0,
                    AdjusterName = s4 != null ? s4.Name : "",
                    AdjusterGroupId = s6 != null ? s6.Id : 0,
                    CaseStatus = o.Status.Description,
                    InsurerRef = s2 != null ? s2.ReferenceNo : "",
                    LawyerRef = s3 != null ? s3.ReferenceNo : "",
                    LawyerCompanyId = s3 != null ? s3.LawFirmId : 0,
                    AgingDays = (currentDate - o.AssignTime).Days,
                    DueDate = o.AssignTime.AddDays(14),
                    CaseNo = o.CaseNo,
                })
                 .WhereIf(input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                 .WhereIf(input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                 .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                 .WhereIf(input.AdjusterIDFilter != null, e => e.AdjusterId == input.AdjusterIDFilter)
                 .WhereIf(input.AdjusterGroupIDFilter != null, e => e.AdjusterGroupId == input.AdjusterGroupIDFilter)
                 .WhereIf(input.LawyerCompanyIDFilter != null, e => e.LawyerCompanyId == input.LawyerCompanyIDFilter)
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VehicleNo.Contains(input.Filter));




            var totalCount = await filteredWIPReports.CountAsync();

            var pagedAndFilteredWIPReports = filteredWIPReports.OrderBy(input.Sorting ?? "Id asc").PageBy(input);
            var wipReportList = await pagedAndFilteredWIPReports.ToListAsync();
            var results = await pagedAndFilteredWIPReports
                            .Select(o => new GetWIPReportForViewDto
                            {
                                WIPReport = new WIPReportDto
                                {
                                    ReportDate = o.ReportDate,
                                    VehicleNo = o.VehicleNo,
                                    CaseReference = o.Id,
                                    InsuranceCompany = o.InsuranceCompany,
                                    CaseType = o.CaseType,
                                    AdjusterName = o.AdjusterName,
                                    InsurerRef = o.InsurerRef,
                                    LawyerRef = o.LawyerRef,
                                    CaseStatus = o.CaseStatus,
                                    AgingDays = o.AgingDays,
                                    DueDate = o.DueDate,
                                    Id = o.Id,
                                    CaseNo = o.CaseNo,
                                }
                            }).ToListAsync();

            return new PagedResultDto<GetWIPReportForViewDto>(
                totalCount,
                results
            );
        }

        public virtual async Task<FileDto> GetWIPReportsToExcel(GetAllWIPReportsForExcelInput input)
        {
            var currentDate = DateTime.Now;
            var filteredWIPReports = (from o in _mainRegistrationRepository.GetAll()
                                      where o.StatusId != 4 && o.StatusId != 5  //Filter out (Completed Invoices, Cancelled) 
                                      join caseInsurer in _caseInsurerRepository.GetAll() on o.Id equals caseInsurer.RegisterId into j2
                                      from s2 in j2.DefaultIfEmpty()
                                      join caseLawyer in _caseLawyerRepository.GetAll() on o.Id equals caseLawyer.RegisterId into j3
                                      from s3 in j3.DefaultIfEmpty()
                                      join user in _lookup_userRepository.GetAll() on o.AdjusterMemberId equals user.Id into j4
                                      from s4 in j4.DefaultIfEmpty()
                                      join staff in _lookup_staffRepository.GetAll() on s4.Id equals staff.UserId into j5
                                      from s5 in j5.DefaultIfEmpty()
                                      join groups in _lookup_groupRepository.GetAll() on s5.GroupId equals groups.Id into j6
                                      from s6 in j6.DefaultIfEmpty()
                                      select new WIPReportDto
                                      {
                                          Id = o.Id,
                                          ReportDate = o.AssignTime,
                                          VehicleNo = o.VehicleNo,
                                          CaseReference = o.Id,
                                          InsuranceCompanyId = o.CompanyId,
                                          InsuranceCompany = o.Company.ShortName,
                                          CaseType = o.CaseType.ShortName,
                                          AdjusterId = s4 != null ? s4.Id : 0,
                                          AdjusterName = s4 != null ? s4.Name : "",
                                          AdjusterGroupId = s6 != null ? s6.Id : 0,
                                          CaseStatus = o.Status.Description,
                                          InsurerRef = s2 != null ? s2.ReferenceNo : "",
                                          LawyerRef = s3 != null ? s3.ReferenceNo : "",
                                          LawyerCompanyId = s3 != null ? s3.LawFirmId : 0,
                                          AgingDays = (currentDate - o.AssignTime).Days,
                                          DueDate = o.AssignTime.AddDays(14),
                                          CaseNo = o.CaseNo,
                                      })
                .WhereIf(input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                .WhereIf(input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                .WhereIf(input.AdjusterIDFilter != null, e => e.AdjusterId == input.AdjusterIDFilter)
                .WhereIf(input.AdjusterGroupIDFilter != null, e => e.AdjusterGroupId == input.AdjusterGroupIDFilter)
                .WhereIf(input.LawyerCompanyIDFilter != null, e => e.LawyerCompanyId == input.LawyerCompanyIDFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VehicleNo.Contains(input.Filter));

            var results = await filteredWIPReports
                .Select(o=> new GetWIPReportForViewDto
                {
                    WIPReport = new WIPReportDto
                    {
                        ReportDate = o.ReportDate,
                        VehicleNo = o.VehicleNo,
                        CaseReference = o.Id,
                        InsuranceCompany = o.InsuranceCompany,
                        CaseType = o.CaseType,
                        AdjusterName = o.AdjusterName,
                        InsurerRef = o.InsurerRef,
                        LawyerRef = o.LawyerRef,
                        CaseStatus = o.CaseStatus,
                        AgingDays = o.AgingDays,
                        DueDate = o.DueDate,
                        Id = o.Id,
                        CaseNo = o.CaseNo,
                    }
                }).ToListAsync();

            return _wipReportsExcelExporter.ExportToFile(results);
        }

    }
}