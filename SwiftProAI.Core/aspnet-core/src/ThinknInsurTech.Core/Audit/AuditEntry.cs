using ThinknInsurTech.Audit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Audit
{
    [Table("AuditEntries")]
    public class AuditEntry : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string NewValue { get; set; }

        public virtual int? AuditTrailId { get; set; }

        [ForeignKey("AuditTrailId")]
        public AuditTrail AuditTrailFk { get; set; }

    }
}