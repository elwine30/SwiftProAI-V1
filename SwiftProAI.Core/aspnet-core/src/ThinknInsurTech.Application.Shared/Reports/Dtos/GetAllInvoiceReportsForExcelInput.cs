using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAllInvoiceReportsForExcelInput
    {
        public ReportDateTypeEnum? DateTypeFilter { get; set; }

        public DateTime? MaxReportDateFilter { get; set; }
        public DateTime? MinReportDateFilter { get; set; }

        public int? InsuranceCompanyFilter { get; set; }

        public int? AdjusterIdFilter { get; set; }

        public InvoiceTypeEnum? InvoiceTypeFilter { get; set; }

        public int? GroupIdFilter { get; set; }

    }
}