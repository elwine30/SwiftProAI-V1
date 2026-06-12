using System;

namespace ThinknInsurTech.Reports
{
    public class WIPReport
    {
        public int? TenantId { get; set; }

        public DateTime ReportDate { get; set; }

        public string VehicleNo { get; set; }

        public int CaseReference { get; set; }

        public string InsuranceCompany { get; set; }

        public string CaseType { get; set; }

        public int AdjusterID { get; set; }

        public string Remark { get; set; }

        public string InsurerRef { get; set; }

        public string LawyerRef { get; set; }

        public string CaseStatus { get; set; }

        public int AgingDays { get; set; }

        public DateTime DueDate { get; set; }

    }
}