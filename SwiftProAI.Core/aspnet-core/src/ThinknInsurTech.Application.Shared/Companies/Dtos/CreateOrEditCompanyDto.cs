using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ThinknInsurTech.Approval.Dtos;

namespace ThinknInsurTech.Companies.Dtos
{
    public class CreateOrEditCompanyDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CompanyConsts.MaxNameLength, MinimumLength = CompanyConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxShortNameLength, MinimumLength = CompanyConsts.MinShortNameLength)]
        public string ShortName { get; set; }

        [Range(CompanyConsts.MinClaimRateValue, CompanyConsts.MaxClaimRateValue)]
        public decimal ClaimRate { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxAddressLength, MinimumLength = CompanyConsts.MinAddressLength)]
        public string Address { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxGstRegNoLength, MinimumLength = CompanyConsts.MinGstRegNoLength)]
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