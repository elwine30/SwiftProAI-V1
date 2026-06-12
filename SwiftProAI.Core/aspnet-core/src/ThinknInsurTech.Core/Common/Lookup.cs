using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Common
{
    [Table("Lookups")]
    [Auditable]
    public class Lookup : Entity
    {
        public int? TenantId { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string Code { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string Description { get; set; }

        [AuditedTrail]
        public virtual bool Active { get; set; }

        [AuditedTrail]
        public virtual int Sequence { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string Group { get; set; }

    }
}