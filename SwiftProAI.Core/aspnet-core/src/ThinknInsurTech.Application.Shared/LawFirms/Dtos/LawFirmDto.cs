using System;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Approval.Dtos;

namespace ThinknInsurTech.LawFirms.Dtos
{
    public class LawFirmDto : EntityDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Address { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public bool IsActive { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        public CreateOrEditViewThirdPartyCaseRequestDto ViewThirdPartyCaseRequest { get; set; }
    }
}