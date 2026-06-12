using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;

using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICasePoliceReportSummariesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCasePoliceReportSummaryForViewDto>> GetAll(GetAllCasePoliceReportSummariesInput input);

        Task<GetCasePoliceReportSummaryForEditOutput> GetCasePoliceReportSummaryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCasePoliceReportSummaryDto input);

        Task Delete(EntityDto input);

    }
}