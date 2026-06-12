using ThinknInsurTech.Registration;
using ThinknInsurTech.Companies;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseThirdPartyVehicles")]
    [Auditable]
    public class CaseThirdPartyVehicle : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(CaseThirdPartyVehicleConsts.MaxVehicleNoLength, MinimumLength = CaseThirdPartyVehicleConsts.MinVehicleNoLength)]
        [AuditedTrail]
        public string VehicleNo { get; set; }


        [StringLength(CaseThirdPartyVehicleConsts.MaxRegisteredOwnerLength, MinimumLength = CaseThirdPartyVehicleConsts.MinRegisteredOwnerLength)]
        [AuditedTrail]
        public string RegisteredOwner { get; set; }

        
        [StringLength(CaseThirdPartyVehicleConsts.MaxVehicleMakeLength, MinimumLength = CaseThirdPartyVehicleConsts.MinVehicleMakeLength)]
        [AuditedTrail]
        public string VehicleMake { get; set; }

        [Range(CaseThirdPartyVehicleConsts.MinVehicleYearValue, CaseThirdPartyVehicleConsts.MaxVehicleYearValue)]
        [AuditedTrail]
        public int? VehicleYear { get; set; }

        [StringLength(CaseThirdPartyVehicleConsts.MaxPolicyNoLength, MinimumLength = CaseThirdPartyVehicleConsts.MinPolicyNoLength)]
        [AuditedTrail]
        public string PolicyNo { get; set; }


        [StringLength(CaseThirdPartyVehicleConsts.MaxTypeCoverLength, MinimumLength = CaseThirdPartyVehicleConsts.MinTypeCoverLength)]
        [AuditedTrail]
        public string TypeCover { get; set; }

        [AuditedTrail]
        public DateTime? CoverStartDate { get; set; }

        [AuditedTrail]
        public DateTime? CoverEndDate { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public virtual int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public InsuranceCompany CompanyFk { get; set; }

        [AuditedTrail]
        public Guid? DriverCarGrant { get; set; } //File, (BinaryObjectId)

        public long? OrganizationUnitId { get; set; }

    }
}