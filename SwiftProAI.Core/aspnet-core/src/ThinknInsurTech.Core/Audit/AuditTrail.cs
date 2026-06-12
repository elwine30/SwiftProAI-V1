using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Audit
{
    [Table("AuditTrails")]
    public abstract class AuditTrailBase : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Operation { get; set; }

        public string TableName { get; set; }

        public string ChangedBy { get; set; }

        public long? OrganizationUnit { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}