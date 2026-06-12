using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using ThinknInsurTech.Audit;
using Abp.Organizations;
using ThinknInsurTech.Approval;

namespace ThinknInsurTech.Workshops
{
    [Table("Workshops")]
    [Audited]
    [Auditable]
    public class Workshop : FullAuditedEntity, IMayHaveTenant , IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        [AuditedTrail]
        public virtual string WorkshopNo { get; set; }

        [AuditedTrail]
        public virtual string WorkshopName { get; set; }

        [AuditedTrail]
        public virtual string Address { get; set; }

        [AuditedTrail]
        public virtual double ClaimRate { get; set; }

        [AuditedTrail]
        public virtual bool IsActive { get; set; }
        public long? OrganizationUnitId { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        [ForeignKey("ViewThirdPartyCaseRequestId")]
        public ViewThirdPartyCaseRequest? ViewThirdPartyCaseRequestFk { get; set; }
    }
}