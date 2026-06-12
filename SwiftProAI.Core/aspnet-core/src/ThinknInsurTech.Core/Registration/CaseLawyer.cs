using ThinknInsurTech.Registration;
using ThinknInsurTech.LawFirms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseLawyers")]
    [Auditable]
    public class CaseLawyer : FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        [AuditedTrail]
        public virtual DateTime HearingDate { get; set; }

        [AuditedTrail]
        public virtual string ReferenceNo { get; set; }

        [AuditedTrail]
        public virtual string ContactNo { get; set; }

        [AuditedTrail]
        public virtual string ContactName { get; set; }

        [AuditedTrail]
        public virtual string Email { get; set; }

        [AuditedTrail]
        public virtual string Type { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public virtual int LawFirmId { get; set; }

        [ForeignKey("LawFirmId")]
        public LawFirm LawFirmFk { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}