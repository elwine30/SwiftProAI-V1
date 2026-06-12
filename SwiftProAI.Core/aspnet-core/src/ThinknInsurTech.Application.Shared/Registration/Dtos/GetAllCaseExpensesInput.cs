using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseExpensesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

        public double? MaxApprovedAmountFilter { get; set; }
        public double? MinApprovedAmountFilter { get; set; }

        public string RemarkFilter { get; set; }

        public string LookupDescriptionFilter { get; set; }

        public string LookupDescription2Filter { get; set; }

        public string LookupDescription3Filter { get; set; }

        public string LookupDescription4Filter { get; set; }

        public string UserNameFilter { get; set; }

    }
}