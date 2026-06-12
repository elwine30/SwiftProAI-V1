using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dto
{
    public class GetCaseExpenseAdjusterViewDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } 

        public string TypeOfExpenses { get; set; }
    }

}
