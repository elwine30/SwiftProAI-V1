using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Reports.Dtos
{
    public class WIPReportDto : EntityDto
    {
        public DateTime ReportDate { get; set; }

        public string VehicleNo { get; set; }

        public int CaseReference { get; set; }

        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompany { get; set; }

        public string CaseType { get; set; }

        public long AdjusterId { get; set; }

        public string AdjusterName { get; set; }

        public int AdjusterGroupId { get; set; }

        public string InsurerRef { get; set; }

        public string LawyerRef { get; set; }

        public int LawyerCompanyId { get; set; }

        public string CaseStatus { get; set; }

        public int AgingDays { get; set; }

        public DateTime DueDate { get; set; }
        public string CaseNo { get; set; }
    }
}