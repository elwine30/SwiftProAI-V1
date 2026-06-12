using System;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAdjusterReportForViewDto
    {
        public DateTime CreatedDate { get; set; }
        public int CaseId { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsuranceCaseRef { get; set; }
        public string CaseType { get; set; }
        public string VehicleNo { get; set; }
        public decimal ServiceFee { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string CaseNo { get; set; }


    }
}