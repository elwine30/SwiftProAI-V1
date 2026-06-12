using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Registration
{
    [Table("ScopeAssignments")]
    [Auditable]
    public class ScopeAssignment : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string Description { get; set; }

        [AuditedTrail]
        public virtual bool isActive { get; set; }

    }
}