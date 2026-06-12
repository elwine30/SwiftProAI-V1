using ThinknInsurTech.Common;
using ThinknInsurTech.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseExpenses")]
    [Auditable]
    public class CaseExpense : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [AuditedTrail]
        public virtual double Amount { get; set; }

        [AuditedTrail]
        public virtual double ApprovedAmount { get; set; }

        [AuditedTrail]
        public virtual string Remark { get; set; }

        public bool Approved { get; set; }
        public bool Rejected { get;set; }

        public virtual int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Lookup StatusFk { get; set; }

        [AuditedTrail]
        public virtual int? TypeId { get; set; }

        [ForeignKey("TypeId")]
        public Lookup TypeFk { get; set; }

        [AuditedTrail]
        public virtual int? SubTypeId { get; set; }

        [ForeignKey("SubTypeId")]
        public Lookup SubTypeFk { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public virtual long AdjusterId { get; set; }

        [ForeignKey("AdjusterId")]
        public User AdjusterFk { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}