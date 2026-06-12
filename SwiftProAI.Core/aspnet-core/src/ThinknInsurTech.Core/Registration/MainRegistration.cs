using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ThinknInsurTech.Audit;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Case;
using ThinknInsurTech.Companies;


namespace ThinknInsurTech.Registration
{
    [Table("MainRegistration")]
    [Auditable]
    public class MainRegistration : FullAuditedEntity<int>, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        [AuditedTrail]
        public int CaseTypeId { get; set; }

        [ForeignKey("CaseTypeId")]
        public CaseType CaseType { get; set; }

        [AuditedTrail]
        public long MemberId { get; set; }

        [AuditedTrail]
        public int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        [AuditedTrail]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public InsuranceCompany Company { get; set; }

        [AuditedTrail]
        public string VehicleNo { get; set; }

        [AuditedTrail]
        public DateTime AssignTime { get; set; }

        [AuditedTrail]
        public DateTime? CompletionTime { get; set; }

        [AuditedTrail]
        public DateTime? ExtendedCompletionDate { get; set; }

        [AuditedTrail]
        public string ExtendCompletionRemark { get; set; }

        [AuditedTrail]
        public string ModeOfAssignment { get; set; }

        [AuditedTrail]
        public long AdjusterMemberId { get; set; }
        [ForeignKey("AdjusterMemberId")]
        public User AdjusterMemberFk { get; set; }

        [AuditedTrail]
        public long? EditorMemberId { get; set; }

        [ForeignKey("EditorMemberId")]
        public User EditorMemberFk { get; set; }

        [AuditedTrail]
        public int? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        public int TenantId { get; set; }

        public string Prefix { get; set; }

        public string CaseNo { get; set; }

        public string FileRefNo { get; set; }
        public long? OrganizationUnitId { get; set; }


        public virtual ICollection<CaseInsuredPerson> InsuredPerson { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<DebitNoteItem> DebitNoteItems { get; set; }
        public virtual ICollection<CreditNoteItem> CreditNoteItems { get; set; }

    }
}
