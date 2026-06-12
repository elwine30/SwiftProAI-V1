using System;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseClaimMainRegistrationInput : PagedAndSortedInputDto
    {
        public DateTime Month { get; set; }
        public DateTime Year { get; set; }
        public string VehicleNumber { get; set; } // list with search
        public string CaseNo { get; set; } //search
    }
}
