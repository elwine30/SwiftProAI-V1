using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Reports
{
    public class InvoiceReport 
    {
        public int? TenantId { get; set; }

        public DateTime ReportDate { get; set; }

        public int CaseReference { get; set; }

        public string InsuranceCompany { get; set; }

        public string InsurerRef { get; set; }

        public string VehicleNo { get; set; }

        public string CaseType { get; set; }

        public int CaseInvoiceId { get; set; }

        public int AdjusterId { get; set; }

    }
}