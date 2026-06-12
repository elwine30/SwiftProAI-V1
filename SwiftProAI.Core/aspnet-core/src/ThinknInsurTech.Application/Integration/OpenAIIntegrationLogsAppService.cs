using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Integration.Dtos;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Integration
{
    [AbpAuthorize(AppPermissions.Pages_Administration_OpenAIIntegrationLogs)]
    public class OpenAIIntegrationLogsAppService : ThinknInsurTechAppServiceBase, IOpenAIIntegrationLogsAppService
    {
        private readonly IRepository<OpenAIIntegrationLog> _openAIIntegrationLogRepository;
        private readonly IRepository<MainRegistration> _mainRegistrationRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;


        public OpenAIIntegrationLogsAppService(IRepository<OpenAIIntegrationLog> openAIIntegrationLogRepository,
            IRepository<MainRegistration> mainRegistrationRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _openAIIntegrationLogRepository = openAIIntegrationLogRepository;
            _mainRegistrationRepository = mainRegistrationRepository;
            _organizationUnitRepository = organizationUnitRepository;

        }

        public virtual async Task<PagedResultDto<GetOpenAIIntegrationLogForViewDto>> GetAll(GetAllOpenAIIntegrationLogsInput input)
        {
            var filteredOpenAIIntegrationLogs = _openAIIntegrationLogRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ActionUrl.Contains(input.Filter) || e.Request.Contains(input.Filter) || e.Response.Contains(input.Filter) || e.CaseNo.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ActionUrlFilter), e => e.ActionUrl.Contains(input.ActionUrlFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.RequestFilter), e => e.Request.Contains(input.RequestFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ResponseFilter), e => e.Response.Contains(input.ResponseFilter))
                .WhereIf(input.MinPromptTokenFilter != null, e => e.PromptToken >= input.MinPromptTokenFilter)
                .WhereIf(input.MaxPromptTokenFilter != null, e => e.PromptToken <= input.MaxPromptTokenFilter)
                .WhereIf(input.MinCompletionTokenFilter != null, e => e.CompletionToken >= input.MinCompletionTokenFilter)
                .WhereIf(input.MaxCompletionTokenFilter != null, e => e.CompletionToken <= input.MaxCompletionTokenFilter)
                .WhereIf(input.MinTotalCostFilter != null, e => e.TotalCost >= input.MinTotalCostFilter)
                .WhereIf(input.MaxTotalCostFilter != null, e => e.TotalCost <= input.MaxTotalCostFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNoFilter), e => e.CaseNo.Contains(input.CaseNoFilter))
                .Join(_mainRegistrationRepository.GetAll(),
                    log => Convert.ToInt32(log.CaseNo), // Convert CaseNo to int
                    mainReg => mainReg.Id,          // Match CaseNo with MainRegistration Id
                    (log, mainReg) => new { log, mainReg })
                .Join(_organizationUnitRepository.GetAll(),
                    combined => combined.mainReg.OrganizationUnitId, // Match OrganizationUnitId from MainRegistration
                    orgUnit => orgUnit.Id,                           // With Id from OrganizationUnit
                    (combined, orgUnit) => new
                    {
                        combined.log,        // OpenAI Integration Log
                        combined.mainReg,    // Main Registration
                        orgUnit,             // Organization Unit
                        combined.mainReg.CreationTime // Assume this field is available
                    })
                .WhereIf(input.OUIdFilter != null && input.OUIdFilter > 0, o => o.orgUnit.Id == input.OUIdFilter)
                .WhereIf(input.MinDateFilter != null, o => o.CreationTime >= input.MinDateFilter)
                .WhereIf(input.MaxDateFilter != null, o => o.CreationTime <= input.MaxDateFilter);

            var pagedAndFilteredOpenAIIntegrationLogs = filteredOpenAIIntegrationLogs
                .OrderBy(input.Sorting ?? "log.Id asc")
                .PageBy(input);

            var openAIIntegrationLogs = from o in pagedAndFilteredOpenAIIntegrationLogs
                                        select new
                                        {
                                            o.log.ActionUrl,
                                            o.log.Request,
                                            o.log.Response,
                                            o.log.PromptToken,
                                            o.log.CompletionToken,
                                            o.log.TotalCost,
                                            o.log.CaseNo,
                                            Id = o.log.Id,
                                            OrganizationUnitName = o.orgUnit.DisplayName, // Assume DisplayName exists
                                            OrganizationUnitId = o.orgUnit.Id,
                                            CreatedDate = o.log.CreationTime // Assume CreationTime exists in MainRegistration
                                        };

            var totalCount = await filteredOpenAIIntegrationLogs.CountAsync();

            var dbList = await openAIIntegrationLogs.ToListAsync();
            var results = new List<GetOpenAIIntegrationLogForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOpenAIIntegrationLogForViewDto()
                {
                    OpenAIIntegrationLog = new OpenAIIntegrationLogDto
                    {
                        ActionUrl = o.ActionUrl,
                        Request = o.Request,
                        Response = o.Response,
                        PromptToken = o.PromptToken,
                        CompletionToken = o.CompletionToken,
                        TotalCost = o.TotalCost,
                        CaseNo = o.CaseNo,
                        Id = o.Id,
                    },
                    OrganizationUnitName = o.OrganizationUnitName,
                    OrganizationUnitId = o.OrganizationUnitId,
                    createdDate = o.CreatedDate
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOpenAIIntegrationLogForViewDto>(
                totalCount,
                results
            );
        }


    }
}