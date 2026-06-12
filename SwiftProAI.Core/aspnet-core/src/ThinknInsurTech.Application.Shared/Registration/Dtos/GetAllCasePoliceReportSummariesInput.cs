using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCasePoliceReportSummariesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxRegisterIdFilter { get; set; }
        public int? MinRegisterIdFilter { get; set; }

        public string ReportSummaryFilter { get; set; }

        public string ReportInconsistencyFilter { get; set; }

    }
}