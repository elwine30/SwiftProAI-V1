using ThinknInsurTech.Branches;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Common
{
    [Table("Groups")]
    [Auditable]
    public class Group : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(GroupConsts.MaxNameLength, MinimumLength = GroupConsts.MinNameLength)]
        [AuditedTrail]
        public string Name { get; set; }

        [Required]
        [Range(GroupConsts.MinGroupTypeValue, GroupConsts.MaxGroupTypeValue)]
        [AuditedTrail]
        public int GroupType { get; set; }

        [AuditedTrail]
        public bool IsActive { get; set; }

        [AuditedTrail]
        public virtual int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch BranchFk { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}