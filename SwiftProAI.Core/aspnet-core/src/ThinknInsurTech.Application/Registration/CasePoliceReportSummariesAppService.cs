using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CasePoliceReportSummaries)]
    public class CasePoliceReportSummariesAppService : ThinknInsurTechAppServiceBase, ICasePoliceReportSummariesAppService
    {
        private readonly IRepository<CasePoliceReportSummary> _casePoliceReportSummaryRepository;
        private readonly IRepository<CasePoliceReport> _casePoliceReportRepository;
        private readonly IRepository<MainRegistration> _mainRegistrationRepository;
        private readonly IOCRPromptService _OCRPromptService;

        public CasePoliceReportSummariesAppService(IRepository<CasePoliceReportSummary> casePoliceReportSummaryRepository, IRepository<CasePoliceReport> casePoliceReportRepository, IOCRPromptService ocrPromptService, IRepository<MainRegistration> mainRegistrationRepository)
        {
            _casePoliceReportSummaryRepository = casePoliceReportSummaryRepository;
            _casePoliceReportRepository = casePoliceReportRepository;
            _OCRPromptService = ocrPromptService;
            _mainRegistrationRepository = mainRegistrationRepository;
        }

        public virtual async Task<PagedResultDto<GetCasePoliceReportSummaryForViewDto>> GetAll(GetAllCasePoliceReportSummariesInput input)
        {

            var filteredCasePoliceReportSummaries = _casePoliceReportSummaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReportSummary.Contains(input.Filter) || e.ReportInconsistency.Contains(input.Filter))
                        .WhereIf(input.MinRegisterIdFilter != null, e => e.RegisterId >= input.MinRegisterIdFilter)
                        .WhereIf(input.MaxRegisterIdFilter != null, e => e.RegisterId <= input.MaxRegisterIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReportSummaryFilter), e => e.ReportSummary.Contains(input.ReportSummaryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReportInconsistencyFilter), e => e.ReportInconsistency.Contains(input.ReportInconsistencyFilter));

            var pagedAndFilteredCasePoliceReportSummaries = filteredCasePoliceReportSummaries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var casePoliceReportSummaries = from o in pagedAndFilteredCasePoliceReportSummaries
                                            select new
                                            {

                                                o.RegisterId,
                                                o.ReportSummary,
                                                o.ReportInconsistency,
                                                Id = o.Id
                                            };

            var totalCount = await filteredCasePoliceReportSummaries.CountAsync();

            var dbList = await casePoliceReportSummaries.ToListAsync();
            var results = new List<GetCasePoliceReportSummaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCasePoliceReportSummaryForViewDto()
                {
                    CasePoliceReportSummary = new CasePoliceReportSummaryDto
                    {

                        RegisterId = o.RegisterId,
                        ReportSummary = o.ReportSummary,
                        ReportInconsistency = o.ReportInconsistency,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCasePoliceReportSummaryForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReportSummaries_Edit)]
        public virtual async Task<GetCasePoliceReportSummaryForEditOutput> GetCasePoliceReportSummaryForEdit(EntityDto input)
        {
            var casePoliceReportSummary = await _casePoliceReportSummaryRepository.GetAll()
                .Where(x => x.RegisterId.Equals(input.Id))
                .OrderByDescending(x => x.LastModificationTime ?? x.CreationTime)
                .FirstOrDefaultAsync();

            var output = new GetCasePoliceReportSummaryForEditOutput { CasePoliceReportSummary = ObjectMapper.Map<CreateOrEditCasePoliceReportSummaryDto>(casePoliceReportSummary) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCasePoliceReportSummaryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReportSummaries_Create)]
        protected virtual async Task Create(CreateOrEditCasePoliceReportSummaryDto input)
        {
            var casePoliceReportSummary = ObjectMapper.Map<CasePoliceReportSummary>(input);

            if (AbpSession.TenantId != null)
            {
                casePoliceReportSummary.TenantId = (int?)AbpSession.TenantId;
            }

            await _casePoliceReportSummaryRepository.InsertAsync(casePoliceReportSummary);

        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReportSummaries_Edit)]
        protected virtual async Task Update(CreateOrEditCasePoliceReportSummaryDto input)
        {
            var casePoliceReportSummary = await _casePoliceReportSummaryRepository.FirstOrDefaultAsync(x => x.SummaryType == input.SummaryType);

            if (casePoliceReportSummary != null)
            {
                input.Id = casePoliceReportSummary.Id;
                ObjectMapper.Map(input, casePoliceReportSummary);
            }
            else
            {
                input.Id = null;
                await Create(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReportSummaries_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _casePoliceReportSummaryRepository.DeleteAsync(input.Id);
        }

        public async Task<string> GenerateReportSummary(int id)
        {
            var casePoliceReports = _casePoliceReportRepository.GetAll().Where(x => x.RegisterId == id);
            var inputData = "";
            foreach (var casePoliceReport in casePoliceReports)
            {
                inputData = inputData + "\n" + casePoliceReport.ReportType + " - " + casePoliceReport.Statement + "\n";
            }
            //var caseNo = _mainRegistrationRepository.GetAll().Where(x => x.Id == id).Select(x => x.CaseNo).ToString();
            var caseNo = await _mainRegistrationRepository.FirstOrDefaultAsync(x => x.Id == id);
            var output = await _OCRPromptService.GenerateSummary(inputData, "GeneratePoliceReportSummary", caseNo != null ? caseNo.CaseNo : "");


            return output;
        }

    }
}