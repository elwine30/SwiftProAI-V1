using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetExpensesApprovalForViewDTO
    {
        public int Id { get; set; }
        public int RegisterId { get; set; }
        public string Status { get; set; }
        public string VehicleNo { get; set; }
        public string Adjuster { get; set; }
        public double Amount { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public bool Approved { get; set; }
        public bool Rejected { get; set; }
        public string CaseNo { get; set; }
    }
}
