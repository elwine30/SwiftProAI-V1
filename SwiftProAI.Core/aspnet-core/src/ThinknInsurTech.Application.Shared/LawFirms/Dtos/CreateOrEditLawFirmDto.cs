using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ThinknInsurTech.Approval.Dtos;

namespace ThinknInsurTech.LawFirms.Dtos
{
    public class CreateOrEditLawFirmDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        public CreateOrEditViewThirdPartyCaseRequestDto ViewThirdPartyCaseRequest { get; set; }
    }
}