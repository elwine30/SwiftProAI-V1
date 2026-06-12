using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Branches
{
    [Table("Branch")]
    [Auditable]
    public class Branch : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }
        [AuditedTrail]
        public virtual string Name { get; set; }
        [AuditedTrail]
        public virtual string ShortName { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}