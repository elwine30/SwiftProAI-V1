using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Registration
{
    [Table("CasePoliceReportSummaries")]
    public class CasePoliceReportSummary : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public int RegisterId { get; set; }

        [StringLength(CasePoliceReportSummaryConsts.MaxReportSummaryLength, MinimumLength = CasePoliceReportSummaryConsts.MinReportSummaryLength)]
        public string ReportSummary { get; set; }

        public string ReportInconsistency { get; set; }

        public string SummaryType { get; set; }
    }
}