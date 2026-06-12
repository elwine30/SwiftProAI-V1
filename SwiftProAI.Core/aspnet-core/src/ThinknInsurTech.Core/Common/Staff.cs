using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Common
{
    [Table("Staffs")]
    [Auditable]
    public class Staff : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [StringLength(StaffConsts.MaxNRICLength, MinimumLength = StaffConsts.MinNRICLength)]
        [AuditedTrail]
        public string NRIC { get; set; }

        [StringLength(StaffConsts.MaxAddressLength, MinimumLength = StaffConsts.MinAddressLength)]
        [AuditedTrail]
        public string Address { get; set; }

        [StringLength(StaffConsts.MaxPassportLength, MinimumLength = StaffConsts.MinPassportLength)]
        [AuditedTrail]
        public string Passport { get; set; }

        [Range(StaffConsts.MinServiceFeePercentValue, StaffConsts.MaxServiceFeePercentValue)]
        [AuditedTrail]
        public decimal? ServiceFeePercent { get; set; }

        [Range(StaffConsts.MinFraudFeePercentValue, StaffConsts.MaxFraudFeePercentValue)]
        [AuditedTrail]
        public decimal? FraudFeePercent { get; set; }

        [AuditedTrail]
        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        [AuditedTrail]
        public virtual int? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group GroupFk { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}