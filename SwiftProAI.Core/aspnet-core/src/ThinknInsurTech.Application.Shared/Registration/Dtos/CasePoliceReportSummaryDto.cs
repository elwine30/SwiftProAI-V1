using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CasePoliceReportSummaryDto : EntityDto
    {
        public int RegisterId { get; set; }

        public string ReportSummary { get; set; }

        public string ReportInconsistency { get; set; }

    }
}