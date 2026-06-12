using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Reports
{
    public class CaseReport
    {
        public int? TenantId { get; set; }

        public string ReportFilter { get; set; }

        public string ReportType { get; set; }

        public DateTime MonthRange { get; set; }

    }
}