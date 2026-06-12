using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;
using ThinknInsurTech.Approval;

namespace ThinknInsurTech.LawFirms
{
    [Table("LawFirm")]
    [Auditable]
    public class LawFirm : FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string Name { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string ShortName { get; set; }

        [Required]
        [AuditedTrail]
        public virtual string Address { get; set; }

        [AuditedTrail]
        public bool IsActive { get; set; }

        public long? OrganizationUnitId { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public long? AssignOUId { get; set; }

        public bool AllowToViewAssignedCases { get; set; }

        public int? ViewThirdPartyCaseRequestId { get; set; }

        [ForeignKey("ViewThirdPartyCaseRequestId")]
        public ViewThirdPartyCaseRequest? ViewThirdPartyCaseRequestFk { get; set; }
    }
}