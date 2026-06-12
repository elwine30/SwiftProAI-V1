using ThinknInsurTech.Registration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseSearchFees")]
    [Auditable]
    public class CaseSearchFee : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(CaseSearchFeeConsts.MaxRemarkLength, MinimumLength = CaseSearchFeeConsts.MinRemarkLength)]
        [AuditedTrail]
        public string Remark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Search Fee Amount must be a positive number")]
        [AuditedTrail]
        public decimal Amount { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}