using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Approval.Dtos
{
    public class GetAllViewThirdPartyCaseRequestsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string Status { get; set; }
        public DateTime? CreationDate { get; set; }

    }
}