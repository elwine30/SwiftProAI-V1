using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAllCaseReportsInput : PagedAndSortedResultRequestDto
    {
        public string ReportFilter { get; set; }

        public string ReportTypeFilter { get; set; }

        public DateTime? MaxMonthRangeFilter { get; set; }
        public DateTime? MinMonthRangeFilter { get; set; }

    }
}