using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Reports.Dtos
{
    public class CaseReportDto : EntityDto
    {
        public string ReportFilter { get; set; }

        public string ReportType { get; set; }

        public DateTime MonthRange { get; set; }

    }
}