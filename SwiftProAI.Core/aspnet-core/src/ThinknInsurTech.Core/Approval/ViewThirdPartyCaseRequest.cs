using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Approval
{
    [Table("ViewThirdPartyCaseRequests")]
    public class ViewThirdPartyCaseRequest : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Status { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? RejectedDate { get; set; }

        public int? RejectedBy { get; set; }

        public long? AssignByOU { get; set; }

        public long? AssignToOU { get; set; }

        public DateTime? CancelledDate { get; set; }

        public int? CancelledBy { get; set; }

        public string CancelRemark { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public string CompanyName { get; set; }
    }
}