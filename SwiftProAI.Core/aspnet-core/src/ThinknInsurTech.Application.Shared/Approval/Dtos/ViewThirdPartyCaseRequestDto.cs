using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Approval.Dtos
{
    public class ViewThirdPartyCaseRequestDto : EntityDto
    {
        public string BusinessRegistrationNo { get; set; }
        public DateTime CreationDate { get; set; }
        public string RequestedBy { get; set; }
        public string CompanyType { get; set; }
        public string AdjusterCompanyName { get; set; }
        public string Name { get; set; }
        public string ApprovedDate { get; set; }
        public string CancelledDate { get; set; }

    }
}