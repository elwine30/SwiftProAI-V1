using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCasePoliceReportSummaryDto : EntityDto<int?>
    {

        public int RegisterId { get; set; }

        [StringLength(CasePoliceReportSummaryConsts.MaxReportSummaryLength, MinimumLength = CasePoliceReportSummaryConsts.MinReportSummaryLength)]
        public string ReportSummary { get; set; }

        public string ReportInconsistency { get; set; }

        public string SummaryType { get; set; }

    }
}