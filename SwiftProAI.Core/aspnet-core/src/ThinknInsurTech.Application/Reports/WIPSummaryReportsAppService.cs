using ThinknInsurTech.Case;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Companies;

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
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;
using ThinknInsurTech.Registration;
//using DocumentFormat.OpenXml.ExtendedProperties;
using NUglify.Helpers;
using Abp.Json;
using ThinknInsurTech.Reports.Dto;
using Abp.AspNetZeroCore.Net;
using System.IO;

namespace ThinknInsurTech.Reports
{
    [AbpAuthorize(AppPermissions.Pages_WIPSummaryReports)]
    public class WIPSummaryReportsAppService : ThinknInsurTechAppServiceBase, IWIPSummaryReportsAppService
    {
        private readonly IWIPSummaryReportsExcelExporter _wipSummaryReportsExcelExporter;
        private readonly IRepository<CaseType, int> _lookup_caseTypeRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private WipSummaryDataInputDto filterApplied;
        private GetAllWipSummaryDataDto WipSummaryData;

        public WIPSummaryReportsAppService(IWIPSummaryReportsExcelExporter wipSummaryReportsExcelExporter, IRepository<CaseType, int> lookup_caseTypeRepository, IRepository<User, long> lookup_userRepository, IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository, IRepository<MainRegistration,int> lookup_mainRegistrationRepository)
        {
            _wipSummaryReportsExcelExporter = wipSummaryReportsExcelExporter;
            _lookup_caseTypeRepository = lookup_caseTypeRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_insuranceCompanyRepository = lookup_insuranceCompanyRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            filterApplied = new WipSummaryDataInputDto();
            WipSummaryData = new GetAllWipSummaryDataDto(); 
        }
        [AbpAuthorize(AppPermissions.Pages_WIPSummaryReports)]
        public async Task<PagedResultDto<WIPSummaryReportCaseTypeLookupTableDto>> GetAllCaseTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_caseTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ShortName != null && e.ShortName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var caseTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WIPSummaryReportCaseTypeLookupTableDto>();
            foreach (var caseType in caseTypeList)
            {
                lookupTableDtoList.Add(new WIPSummaryReportCaseTypeLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType.ShortName?.ToString()
                });
            }

            return new PagedResultDto<WIPSummaryReportCaseTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_WIPSummaryReports)]
        public async Task<PagedResultDto<WIPSummaryReportUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WIPSummaryReportUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new WIPSummaryReportUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<WIPSummaryReportUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_WIPSummaryReports)]
        public async Task<PagedResultDto<WIPSummaryReportCompanyLookupTableDto>> GetAllCompanyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_insuranceCompanyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ShortName != null && e.ShortName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var companyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WIPSummaryReportCompanyLookupTableDto>();
            foreach (var company in companyList)
            {
                lookupTableDtoList.Add(new WIPSummaryReportCompanyLookupTableDto
                {
                    Id = company.Id,
                    DisplayName = company.ShortName?.ToString()
                });
            }

            return new PagedResultDto<WIPSummaryReportCompanyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<GetAllWipSummaryDataDto> GetAllWipSummaryData(WipSummaryDataInputDto input)
        {
            filterApplied = input;
            var query = _lookup_mainRegistrationRepository.GetAll()
                .WhereIf(input.CaseTypeId != null, e => e.CaseTypeId == input.CaseTypeId)
                .WhereIf(input.UserId != null, e => e.AdjusterMemberId == input.UserId)
                .WhereIf(input.CompanyId != null, e => e.CompanyId == input.CompanyId);

            var data = await query.ToListAsync();

            var pivotTable = GeneratePivotTable(data, input);
            this.WipSummaryData = pivotTable;

            return pivotTable;
        }
        public async Task<FileDto> GetWIPSummaryReportsToExcel(WipSummaryDataInputDto input)
        {

            filterApplied = input;
            var query = _lookup_mainRegistrationRepository.GetAll()
                .WhereIf(input.CaseTypeId != null, e => e.CaseTypeId == input.CaseTypeId)
                .WhereIf(input.UserId != null, e => e.AdjusterMemberId == input.UserId)
                .WhereIf(input.CompanyId != null, e => e.CompanyId == input.CompanyId);

            var data = await query.ToListAsync();

            var pivotTable = GeneratePivotTable(data, input);

            return _wipSummaryReportsExcelExporter.ExportWIPSummaryToFile(pivotTable, input);
        }

        #region HelperMethod
        private GetAllWipSummaryDataDto GeneratePivotTable(List<MainRegistration> data, WipSummaryDataInputDto filter)
        {
            var responseDto = new GetAllWipSummaryDataDto
            {
                ColumnHeaders = new List<string>(),
                RowHeaders = new List<string>(),
                Data = new Dictionary<string, Dictionary<string, int>>()
            };

            var companyNames = data.Select(d => d.CompanyId).Distinct()
                .ToDictionary(id => id, id => _lookup_insuranceCompanyRepository.FirstOrDefault(id)?.Name ?? id.ToString());

            var caseTypeNames = data.Select(d => d.CaseTypeId).Distinct()
                .ToDictionary(id => id, id => _lookup_caseTypeRepository.FirstOrDefault(id)?.ShortName ?? id.ToString());

            var adjusterNames = data.Select(d => d.AdjusterMemberId).Distinct()
                .ToDictionary(id => id, id => _lookup_userRepository.FirstOrDefault(id)?.UserName ?? id.ToString());

            //Amend this that columnheaders equals to casetype data and rowheaders equals to user data
            if (filter.CompanyId.HasValue && filter.CaseTypeId.HasValue && filter.UserId.HasValue)
            {
                responseDto.ColumnHeaders.Add(" ");
                var caseTypes = data.Select(d => d.CaseTypeId).Distinct().ToList();
                responseDto.ColumnHeaders.AddRange(caseTypes.Select(ct => caseTypeNames[ct]));

                var groupedData = data.GroupBy(d => d.AdjusterMemberId).ToList();
                foreach (var group in groupedData)
                {
                    var rowKey = adjusterNames[group.Key];
                    responseDto.RowHeaders.Add(rowKey);

                    foreach (var caseType in caseTypes)
                    {
                        var columnKey = caseTypeNames[caseType];
                        if (!responseDto.Data.ContainsKey(columnKey))
                        {
                            responseDto.Data[columnKey] = new Dictionary<string, int>();
                        }
                        if (!responseDto.Data[columnKey].ContainsKey(rowKey))
                        {
                            responseDto.Data[columnKey][rowKey] = 0;
                        }
                        responseDto.Data[columnKey][rowKey] = group.Count(d => d.CaseTypeId == caseType && d.CompanyId == filter.CompanyId && d.AdjusterMemberId == filter.UserId);
                    }
                }
            }
            else if (filter.CompanyId.HasValue && filter.CaseTypeId.HasValue)
            {
                var adjusters = data.Select(d => d.AdjusterMemberId).Distinct().ToList();
                responseDto.ColumnHeaders.AddRange(adjusters.Select(a => adjusterNames[a]));

                var groupedData = data.GroupBy(d => d.AdjusterMemberId).ToList();
                foreach (var group in groupedData)
                {
                    var rowKey = adjusterNames[group.Key];
                    responseDto.RowHeaders.Add(rowKey);

                    var columnKey = companyNames[filter.CompanyId.Value];
                    if (!responseDto.Data.ContainsKey(columnKey))
                    {
                        responseDto.Data[columnKey] = new Dictionary<string, int>();
                    }
                    if (!responseDto.Data[columnKey].ContainsKey(rowKey))
                    {
                        responseDto.Data[columnKey][rowKey] = 0;
                    }
                    responseDto.Data[columnKey][rowKey] = group.Count(d => d.CaseTypeId == filter.CaseTypeId && d.AdjusterMemberId == group.Key);
                }
            }
            else if (filter.CompanyId.HasValue && filter.UserId.HasValue)
            {
                var companies = data.Select(d => d.CompanyId).Distinct().ToList();
                responseDto.ColumnHeaders.AddRange(companies.Select(c => companyNames[c]));

                var caseTypes = data.Select(d => d.CaseTypeId).Distinct().ToList();
                foreach (var caseType in caseTypes)
                {
                    var rowKey = caseTypeNames[caseType];
                    responseDto.RowHeaders.Add(rowKey);

                    foreach (var companyId in companies)
                    {
                        var columnKey = companyNames[companyId];
                        if (!responseDto.Data.ContainsKey(columnKey))
                        {
                            responseDto.Data[columnKey] = new Dictionary<string, int>();
                        }
                        if (!responseDto.Data[columnKey].ContainsKey(rowKey))
                        {
                            responseDto.Data[columnKey][rowKey] = 0;
                        }
                        responseDto.Data[columnKey][rowKey] = data.Count(d => d.CaseTypeId == caseType && d.CompanyId == companyId && d.AdjusterMemberId == filter.UserId);
                    }
                }
            }
            else if (filter.UserId.HasValue && filter.CaseTypeId.HasValue)
            {
                var companies = data.Select(d => d.CompanyId).Distinct().ToList();
                responseDto.ColumnHeaders.AddRange(companies.Select(c => companyNames[c]));

                var caseTypes = data.Select(d => d.CaseTypeId).Distinct().ToList();
                responseDto.RowHeaders.AddRange(caseTypes.Select(ct => caseTypeNames[ct]));

                foreach (var caseType in caseTypes)
                {
                    var rowKey = caseTypeNames[caseType];

                    foreach (var company in companies)
                    {
                        var columnKey = companyNames[company];
                        if (!responseDto.Data.ContainsKey(columnKey))
                        {
                            responseDto.Data[columnKey] = new Dictionary<string, int>();
                        }
                        if (!responseDto.Data[columnKey].ContainsKey(rowKey))
                        {
                            responseDto.Data[columnKey][rowKey] = 0;
                        }
                        responseDto.Data[columnKey][rowKey] = data.Count(d => d.CaseTypeId == caseType && d.CompanyId == company && d.AdjusterMemberId == filter.UserId);
                    }
                }
            }
            else if (filter.CompanyId.HasValue)
            {
                var adjusters = data.Select(d => d.AdjusterMemberId).Distinct().ToList();
                responseDto.ColumnHeaders.AddRange(adjusters.Select(a => adjusterNames[a]));

                var companyName = companyNames[filter.CompanyId.Value];

                foreach (var adjuster in adjusters)
                {
                    var rowKey = adjusterNames[adjuster];
                    responseDto.RowHeaders.Add(rowKey);

                    if (!responseDto.Data.ContainsKey(companyName))
                    {
                        responseDto.Data[companyName] = new Dictionary<string, int>();
                    }

                    if (!responseDto.Data[companyName].ContainsKey(rowKey))
                    {
                        responseDto.Data[companyName][rowKey] = 0;
                    }

                    responseDto.Data[companyName][rowKey] = data.Count(d => d.AdjusterMemberId == adjuster && d.CompanyId == filter.CompanyId);
                }
            }
            else if (filter.UserId.HasValue)
            {
                var companies = data.Select(d => d.CompanyId).Distinct().ToList();
                responseDto.ColumnHeaders.AddRange(companies.Select(c => companyNames[c]));

                var caseTypes = data.Select(d => d.CaseTypeId).Distinct().ToList();
                responseDto.RowHeaders.AddRange(caseTypes.Select(ct => caseTypeNames[ct]));

                foreach (var caseType in caseTypes)
                {
                    var rowKey = caseTypeNames[caseType];

                    foreach (var company in companies)
                    {
                        var columnKey = companyNames[company];
                        if (!responseDto.Data.ContainsKey(columnKey))
                        {
                            responseDto.Data[columnKey] = new Dictionary<string, int>();
                        }
                        if (!responseDto.Data[columnKey].ContainsKey(rowKey))
                        {
                            responseDto.Data[columnKey][rowKey] = 0;
                        }
                        responseDto.Data[columnKey][rowKey] = data.Count(d => d.CaseTypeId == caseType && d.CompanyId == company && d.AdjusterMemberId == filter.UserId);
                    }
                }
            }
            else if (filter.CaseTypeId.HasValue)
            {
                var companies = _lookup_insuranceCompanyRepository.GetAll().ToList();
                responseDto.ColumnHeaders.AddRange(companies.Select(c => c.Name));

                var adjusters = _lookup_userRepository.GetAll().ToList();
                responseDto.RowHeaders.AddRange(adjusters.Select(a => a.UserName));

                foreach (var adjuster in adjusters)
                {
                    var rowKey = adjuster.UserName;

                    foreach (var company in companies)
                    {
                        var columnKey = company.Name;
                        if (!responseDto.Data.ContainsKey(columnKey))
                        {
                            responseDto.Data[columnKey] = new Dictionary<string, int>();
                        }
                        if (!responseDto.Data[columnKey].ContainsKey(rowKey))
                        {
                            responseDto.Data[columnKey][rowKey] = 0;
                        }
                        responseDto.Data[columnKey][rowKey] = data.Count(d => d.AdjusterMemberId == adjuster.Id && d.CompanyId == company.Id && d.CaseTypeId == filter.CaseTypeId);
                    }
                }
            }
            else
            {
                responseDto = null;
            }

            return responseDto;
        }
        #endregion



    }
}