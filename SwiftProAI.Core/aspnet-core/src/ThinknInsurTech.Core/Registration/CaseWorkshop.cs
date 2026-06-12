using ThinknInsurTech.Registration;
using ThinknInsurTech.Workshops;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseWorkshops")]
    [Auditable]
    public class CaseWorkshop : FullAuditedEntity, IMayHaveTenant , IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        [AuditedTrail]
        public virtual string Email { get; set; }

        [AuditedTrail]
        public virtual string ContactNo { get; set; }

        [AuditedTrail]
        public virtual string ContactName { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public virtual int? WorkshopId { get; set; }

        [ForeignKey("WorkshopId")]
        public Workshop WorkshopFk { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}