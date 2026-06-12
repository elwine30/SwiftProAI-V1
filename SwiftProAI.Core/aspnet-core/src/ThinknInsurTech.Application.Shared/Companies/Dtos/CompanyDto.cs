using Abp.Application.Services.Dto;
using ThinknInsurTech.Approval.Dtos;

namespace ThinknInsurTech.Companies.Dtos
{
    public class CompanyDto : EntityDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public decimal ClaimRate { get; set; }

        public string Address { get; set; }

        public string GstRegNo { get; set; }

        public bool IsActive { get; set; }

        public decimal PhotographCharge { get; set; }

        public int CaseTypeId { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        public CreateOrEditViewThirdPartyCaseRequestDto ViewThirdPartyCaseRequest { get; set; }
    }
}