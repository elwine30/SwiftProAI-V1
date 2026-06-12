using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Reports.Exporting;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Case;
using ThinknInsurTech.Branches;

namespace ThinknInsurTech.Reports
{
    [AbpAuthorize(AppPermissions.Pages_CaseReports)]
    public class CaseReportsAppService : ThinknInsurTechAppServiceBase, ICaseReportsAppService
    {
        private readonly ICaseReportsExcelExporter _caseReportsExcelExporter;
        private readonly IRepository<MainRegistration> _mainRegistrationRepository;
        private readonly IRepository<InsuranceCompany> _insuranceCompanyRepository;
        private readonly IRepository<CaseType> _caseTypeRepository;
        private readonly IRepository<Branch> _branchRepository;
        //private readony IRepository<State> _stateRepository;

        public CaseReportsAppService(ICaseReportsExcelExporter caseReportsExcelExporter, IRepository<MainRegistration> mainRegistrationRepository, IRepository<InsuranceCompany> insuranceCompanyRepository, IRepository<CaseType> caseTypeRepository, IRepository<Branch> branchRepository)
        {
            _caseReportsExcelExporter = caseReportsExcelExporter;
            _mainRegistrationRepository = mainRegistrationRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _caseTypeRepository = caseTypeRepository;
            _branchRepository = branchRepository;
            //_stateRepository = stateRepository
        }
        private List<DateTime> GetMonthRanges(DateTime minDate, DateTime maxDate)
        {
            var monthRanges = new List<DateTime>();
            for (var date = new DateTime(minDate.Year, minDate.Month, 1); date <= maxDate; date = date.AddMonths(1))
            {
                monthRanges.Add(date);
            }
            return monthRanges;
        }

        public virtual async Task<GetCaseReportForViewDto> GetAll(GetAllCaseReportsInput input)
        {
            var monthRanges = GetMonthRanges((DateTime)input.MinMonthRangeFilter, (DateTime)input.MaxMonthRangeFilter);
            var insuranceCompanyList = _insuranceCompanyRepository.GetAll().Select(x => new { Id = x.Id, Value = x.ShortName }).Distinct().ToList();
            var caseTypeList = _caseTypeRepository.GetAll().Select(x => new { Id = x.Id, Value = x.ShortName }).Distinct().ToList();
            var stateList = _branchRepository.GetAll().Select(x=> new {Id = x.Id, Value = x.Name}).Distinct().ToList();
            var isInvoiceReport = input.ReportTypeFilter != null && input.ReportTypeFilter == "invoiceReport";
            var queryCaseReport = _mainRegistrationRepository.GetAll()
                .Where(m => m.AssignTime >= input.MinMonthRangeFilter && m.AssignTime < input.MaxMonthRangeFilter);
            var responseDto = new GetCaseReportForViewDto
            {
                ColumnHeader = new List<string>(),
                RowHeader = new List<string>(),
                ReportData = new Dictionary<string, Dictionary<string, int>>(),
            };

            if (isInvoiceReport)
            {
                queryCaseReport = queryCaseReport.Where(x => x.StatusId == (int?)EnumRegistrationStatus.CompletedInvoices);
            }


            switch (input.ReportFilter)
            {
                case "insuranceCompany":
                    //Add the first header
                    responseDto.ColumnHeader.Add("Insurance Company");
                    foreach (var monthStart in monthRanges) { responseDto.ColumnHeader.Add(monthStart.ToString("yyyy-MM")); }
                    foreach (var company in insuranceCompanyList)
                    {
                        responseDto.RowHeader.Add(company.Value);
                        //Outer Dictionary
                        responseDto.ReportData[company.Value] = new Dictionary<string, int>();
                        foreach (var monthStart in monthRanges)
                        {
                            var monthEnd = monthStart.AddMonths(1);
                            //Inner Dictionary
                            var MonthlyReportData = new Dictionary<string, int>();
                            var reportQuery = queryCaseReport
                                .Where(m => m.AssignTime >= monthStart && m.AssignTime < monthEnd)
                                .Where(c => c.CompanyId == company.Id);
                            var reportMonthCount = reportQuery.Count();
                            responseDto.ReportData[company.Value][monthStart.ToString("yyyy-MM")] = reportMonthCount;
                        }
                    }
                    break;

                case "caseType":
                    //Add the first header
                    responseDto.ColumnHeader.Add("Case Type");
                    foreach (var monthStart in monthRanges) { responseDto.ColumnHeader.Add(monthStart.ToString("yyyy-MM")); }
                    foreach (var caseType in caseTypeList)
                    {
                        responseDto.RowHeader.Add(caseType.Value);
                        responseDto.ReportData[caseType.Value] = new Dictionary<string, int>();
                        foreach (var monthStart in monthRanges)
                        {
                            var monthEnd = monthStart.AddMonths(1);
                            var MonthlyReportData = new Dictionary<string, int>();
                            var reportQuery = queryCaseReport
                                .Where(m => m.AssignTime >= monthStart && m.AssignTime < monthEnd)
                                .Where(c => c.CaseTypeId == caseType.Id);
                            var reportMonthCount = reportQuery.Count();
                            responseDto.ReportData[caseType.Value][monthStart.ToString("yyyy-MM")] = reportMonthCount;
                        }
                    }
                    break;

                case "state":
                    //Add the first header
                    responseDto.ColumnHeader.Add("State");
                    foreach (var monthStart in monthRanges) { responseDto.ColumnHeader.Add(monthStart.ToString("yyyy-MM")); }
                    foreach (var state in stateList)
                    {
                        responseDto.RowHeader.Add(state.Value);
                        responseDto.ReportData[state.Value] = new Dictionary<string, int>();
                        foreach (var monthStart in monthRanges)
                        {
                            var monthEnd = monthStart.AddMonths(1);
                            var MonthlyReportData = new Dictionary<string, int>();
                            var reportQuery = queryCaseReport
                                .Where(m => m.AssignTime >= monthStart && m.AssignTime < monthEnd)
                                .Where(s => s.BranchId == state.Id);
                            var reportMonthCount = reportQuery.Count();
                            responseDto.ReportData[state.Value][monthStart.ToString("yyyy-MM")] = reportMonthCount;
                        }
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid ReportFilter value");
            }

            return responseDto;

        }

        public virtual async Task<FileDto> GetCaseReportsToExcel(GetAllCaseReportsForExcelInput input)
        {
            var monthRanges = GetMonthRanges((DateTime)input.MinMonthRangeFilter, (DateTime)input.MaxMonthRangeFilter);
            var insuranceCompanyList = _insuranceCompanyRepository.GetAll().Select(x => new { Id = x.Id, Value = x.ShortName }).Distinct().ToList();
            var caseTypeList = _caseTypeRepository.GetAll().Select(x => new { Id = x.Id, Value = x.ShortName }).Distinct().ToList();
            var stateList = _branchRepository.GetAll().Select(x => new { Id = x.Id, Value = x.Name }).Distinct().ToList();
            var isInvoiceReport = input.ReportTypeFilter != null && input.ReportTypeFilter == "invoiceReport";
            var queryCaseReport = _mainRegistrationRepository.GetAll()
                .Where(m => m.AssignTime >= input.MinMonthRangeFilter && m.AssignTime < input.MaxMonthRangeFilter);
            var responseDto = new GetCaseReportForViewDto
            {
                ColumnHeader = new List<string>(),
                RowHeader = new List<string>(),
                ReportData = new Dictionary<string, Dictionary<string, int>>(),
            };

            if (isInvoiceReport)
            {
                queryCaseReport = queryCaseReport.Where(x => x.StatusId == (int?)EnumRegistrationStatus.CompletedInvoices);
            }


            switch (input.ReportFilter)
            {
                case "insuranceCompany":
                    //Add the first header
                    responseDto.ColumnHeader.Add("Insurance Company");
                    foreach (var monthStart in monthRanges) { responseDto.ColumnHeader.Add(monthStart.ToString("yyyy-MM")); }
                    foreach (var company in insuranceCompanyList)
                    {
                        responseDto.RowHeader.Add(company.Value);
                        //Outer Dictionary
                        responseDto.ReportData[company.Value] = new Dictionary<string, int>();
                        foreach (var monthStart in monthRanges)
                        {
                            var monthEnd = monthStart.AddMonths(1);
                            //Inner Dictionary
                            var MonthlyReportData = new Dictionary<string, int>();
                            var reportQuery = queryCaseReport
                                .Where(m => m.AssignTime >= monthStart && m.AssignTime < monthEnd)
                                .Where(c => c.CompanyId == company.Id);
                            var reportMonthCount = reportQuery.Count();
                            responseDto.ReportData[company.Value][monthStart.ToString("yyyy-MM")] = reportMonthCount;
                        }
                    }
                    break;

                case "caseType":
                    //Add the first header
                    responseDto.ColumnHeader.Add("Case Type");
                    foreach (var monthStart in monthRanges) { responseDto.ColumnHeader.Add(monthStart.ToString("yyyy-MM")); }
                    foreach (var caseType in caseTypeList)
                    {
                        responseDto.RowHeader.Add(caseType.Value);
                        responseDto.ReportData[caseType.Value] = new Dictionary<string, int>();
                        foreach (var monthStart in monthRanges)
                        {
                            var monthEnd = monthStart.AddMonths(1);
                            var MonthlyReportData = new Dictionary<string, int>();
                            var reportQuery = queryCaseReport
                                .Where(m => m.AssignTime >= monthStart && m.AssignTime < monthEnd)
                                .Where(c => c.CaseTypeId == caseType.Id);
                            var reportMonthCount = reportQuery.Count();
                            responseDto.ReportData[caseType.Value][monthStart.ToString("yyyy-MM")] = reportMonthCount;
                        }
                    }
                    break;

                case "state":
                    //Add the first header
                    responseDto.ColumnHeader.Add("State");
                    foreach (var monthStart in monthRanges) { responseDto.ColumnHeader.Add(monthStart.ToString("yyyy-MM")); }
                    foreach (var state in stateList)
                    {
                        responseDto.RowHeader.Add(state.Value);
                        responseDto.ReportData[state.Value] = new Dictionary<string, int>();
                        foreach (var monthStart in monthRanges)
                        {
                            var monthEnd = monthStart.AddMonths(1);
                            var MonthlyReportData = new Dictionary<string, int>();
                            var reportQuery = queryCaseReport
                                .Where(m => m.AssignTime >= monthStart && m.AssignTime < monthEnd)
                                .Where(s => s.BranchId == state.Id);
                            var reportMonthCount = reportQuery.Count();
                            responseDto.ReportData[state.Value][monthStart.ToString("yyyy-MM")] = reportMonthCount;
                        }
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid ReportFilter value");
            }

            return _caseReportsExcelExporter.ExportCaseReportToFile(responseDto, input);
        }

    }
}