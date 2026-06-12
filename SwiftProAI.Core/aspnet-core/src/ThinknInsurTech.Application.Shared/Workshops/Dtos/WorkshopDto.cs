using System;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Approval.Dtos;

namespace ThinknInsurTech.Workshops.Dtos
{
    public class WorkshopDto : EntityDto
    {
        public string WorkshopNo { get; set; }

        public string WorkshopName { get; set; }

        public string Address { get; set; }

        public double ClaimRate { get; set; }

        public bool IsActive { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        public CreateOrEditViewThirdPartyCaseRequestDto ViewThirdPartyCaseRequest { get; set; }
    }
}