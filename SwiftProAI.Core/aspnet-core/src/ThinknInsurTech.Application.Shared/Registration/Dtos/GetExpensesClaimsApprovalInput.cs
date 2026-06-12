using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetExpensesClaimsApprovalInput : PagedAndSortedResultRequestDto
    {
        public DateTime? DateFromFilter { get; set; }
        public DateTime? DateToFilter { get; set; }
        public string SelectedStatusFilter { get; set; }
        public string SelectedTypeFilter { get; set; }
        public int SelectedGroupFilter { get; set; }
        public long SelectedAdjusterFilter { get; set; }
    }
}
