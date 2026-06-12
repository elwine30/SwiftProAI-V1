using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class ExpensesClaimsApprovalDto
    {
        public int Id { get; set; }
        public bool Approved {  get; set; }
        public bool Rejected { get; set; }
    }
}
