using System;
using System.Collections.Generic;
using System.Text;
using ThinknInsurTech.Remarks.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class ReassignCaseAdjusterDto
    {
        public int RegistrationId { get; set; }
        public int AdjusterId { get; set; }
        public RemarkDto Remark { get; set; }
    }
}
