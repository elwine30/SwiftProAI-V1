using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System.ComponentModel.DataAnnotations.Schema;
using ThinknInsurTech.Audit;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;

namespace ThinknInsurTech.Registration
{
    [Table("CaseAdjusters")]
    [Auditable]
    public class CaseAdjuster : FullAuditedEntity<int>, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }
        [AuditedTrail]
        public string Status { get; set; }
        [AuditedTrail]
        public int? ScopeAssignmentId { get; set; }

        [ForeignKey("ScopeAssignmentId")]
        public ScopeAssignment ScopeAssignmentFk { get; set; }

        public int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }
        [AuditedTrail]
        public int? StateLocationId { get; set; }

        [ForeignKey("StateLocationId")]
        public Location StateLocationFk { get; set; }
        [AuditedTrail]
        public long? EditorUserId { get; set; }

        [ForeignKey("EditorUserId")]
        public User EditorUserFk { get; set; }
        public long? OrganizationUnitId { get; set; }

        public string ScopeAssignmentRemarks { get; set; }

    }
}