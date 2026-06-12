using ThinknInsurTech.Case;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;
using ThinknInsurTech.Approval;

namespace ThinknInsurTech.Companies
{
    [Table("InsuranceCompany")]
    [Auditable]
    public class InsuranceCompany : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxNameLength, MinimumLength = CompanyConsts.MinNameLength)]
        [AuditedTrail]
        public string Name { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxShortNameLength, MinimumLength = CompanyConsts.MinShortNameLength)]
        [AuditedTrail]
        public string ShortName { get; set; }

        [Range(CompanyConsts.MinClaimRateValue, CompanyConsts.MaxClaimRateValue)]
        [AuditedTrail]
        public decimal ClaimRate { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxAddressLength, MinimumLength = CompanyConsts.MinAddressLength)]
        [AuditedTrail]
        public string Address { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxGstRegNoLength, MinimumLength = CompanyConsts.MinGstRegNoLength)]
        [AuditedTrail]
        public string GstRegNo { get; set; }

        [AuditedTrail]
        public bool IsActive { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Photograph Charge must be a positive number")]
        [AuditedTrail]
        public decimal PhotographCharge { get; set; }

        [AuditedTrail]
        public virtual int CaseTypeId { get; set; }

        [ForeignKey("CaseTypeId")]
        public CaseType CaseTypeFk { get; set; }

        public long? OrganizationUnitId { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        [ForeignKey("ViewThirdPartyCaseRequestId")]
        public ViewThirdPartyCaseRequest? ViewThirdPartyCaseRequestFk { get; set; }
    }
}