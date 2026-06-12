using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetClaimsApprovalForViewDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string VehicleNo { get; set; }
        public string Adjuster { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public bool Approved { get; set; }
        public bool Rejected { get; set; }
        public string CaseNo { get; set; }
    }
}
