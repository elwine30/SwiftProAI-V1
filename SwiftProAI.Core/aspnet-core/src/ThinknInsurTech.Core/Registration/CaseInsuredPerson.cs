using ThinknInsurTech.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseInsuredPersons")]
    [Auditable]
    public class CaseInsuredPerson : FullAuditedEntity, IMustHaveTenant , IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [AuditedTrail]
        public bool IsOwner { get; set; }

        [AuditedTrail]
        public bool IsDriver { get; set; }

        [AuditedTrail]
        public bool IsThirdParty { get; set; }

        [AuditedTrail]
        public string Relationship { get; set; }

        [Required]
        [StringLength(InsuredPersonConsts.MaxNameLength, MinimumLength = InsuredPersonConsts.MinNameLength)]
        [AuditedTrail]
        public string Name { get; set; }

        [StringLength(InsuredPersonConsts.MaxIdenticationTypeLength, MinimumLength = InsuredPersonConsts.MinIdenticationTypeLength)]
        [AuditedTrail]
        public string IdenticationType { get; set; }

        [StringLength(InsuredPersonConsts.MaxIdenticationNoLength, MinimumLength = InsuredPersonConsts.MinIdenticationNoLength)]
        [AuditedTrail]
        public string IdenticationNo { get; set; }

        [StringLength(InsuredPersonConsts.MaxContactLength, MinimumLength = InsuredPersonConsts.MinContactLength)]
        [AuditedTrail]
        public string Contact { get; set; }

        [StringLength(InsuredPersonConsts.MaxNationalityLength, MinimumLength = InsuredPersonConsts.MinNationalityLength)]
        [AuditedTrail]
        public string Nationality { get; set; }

        [StringLength(InsuredPersonConsts.MaxMakeLength, MinimumLength = InsuredPersonConsts.MinMakeLength)]
        [AuditedTrail]
        public string Make { get; set; }

        [StringLength(InsuredPersonConsts.MaxModelLength, MinimumLength = InsuredPersonConsts.MinModelLength)]
        [AuditedTrail]
        public string Model { get; set; }

        [StringLength(InsuredPersonConsts.MaxSpecsLength, MinimumLength = InsuredPersonConsts.MinSpecsLength)]
        [AuditedTrail]
        public string Specs { get; set; }

        [AuditedTrail]
        public short? Year { get; set; }

        [AuditedTrail]
        public double? Valuation { get; set; }

        [AuditedTrail]
        public decimal? ValuationEquiry { get; set; }

        [StringLength(InsuredPersonConsts.MaxPolicyNoLength, MinimumLength = InsuredPersonConsts.MinPolicyNoLength)]
        [AuditedTrail]
        public string PolicyNo { get; set; }

        [StringLength(InsuredPersonConsts.MaxCoverageLength, MinimumLength = InsuredPersonConsts.MinCoverageLength)]
        [AuditedTrail]
        public string Coverage { get; set; }

        [StringLength(InsuredPersonConsts.MaxPostcodeLength, MinimumLength = InsuredPersonConsts.MinPostcodeLength)]
        [AuditedTrail]
        public string Postcode { get; set; }

        [StringLength(InsuredPersonConsts.MaxCityLength, MinimumLength = InsuredPersonConsts.MinCityLength)]
        [AuditedTrail]
        public string City { get; set; }

        [StringLength(InsuredPersonConsts.MaxAddressLength, MinimumLength = InsuredPersonConsts.MinAddressLength)]
        [AuditedTrail]
        public string Address { get; set; }

        [StringLength(InsuredPersonConsts.MaxJpjRegisterNoLength, MinimumLength = InsuredPersonConsts.MinJpjRegisterNoLength)]
        [AuditedTrail]
        public string JpjRegisterNo { get; set; }

        [AuditedTrail]
        public DateTime? JpjRegisterDate { get; set; }

        [StringLength(InsuredPersonConsts.MaxOccupationLength, MinimumLength = InsuredPersonConsts.MinOccupationLength)]
        [AuditedTrail]
        public string Occupation { get; set; }

        [StringLength(InsuredPersonConsts.MaxEmployerNameLength, MinimumLength = InsuredPersonConsts.MinEmployerNameLength)]
        [AuditedTrail]
        public string EmployerName { get; set; }

        [StringLength(InsuredPersonConsts.MaxEmployerContactLength, MinimumLength = InsuredPersonConsts.MinEmployerContactLength)]
        [AuditedTrail]
        public string EmployerContact { get; set; }

        [StringLength(InsuredPersonConsts.MaxEmployerAddressLength, MinimumLength = InsuredPersonConsts.MinEmployerAddressLength)]
        [AuditedTrail]
        public string EmployerAddress { get; set; }

        [AuditedTrail]
        public double? MonthlyIncome { get; set; }

        [StringLength(InsuredPersonConsts.MaxLicenseClassesLength, MinimumLength = InsuredPersonConsts.MinLicenseClassesLength)]
        [AuditedTrail]
        public string LicenseClasses { get; set; }

        [StringLength(InsuredPersonConsts.MaxLicenseNoLength, MinimumLength = InsuredPersonConsts.MinLicenseNoLength)]
        [AuditedTrail]
        public string LicenseNo { get; set; }

        [StringLength(InsuredPersonConsts.MaxDrivingExperienceLength, MinimumLength = InsuredPersonConsts.MinDrivingExperienceLength)]
        [AuditedTrail]
        public string DrivingExperience { get; set; }

        [AuditedTrail]
        public DateTime? LicenseDateFrom { get; set; }

        [AuditedTrail]
        public DateTime? LicenseDateTo { get; set; }
        //File

        [AuditedTrail]
        public Guid? DriverICFront { get; set; } //File, (BinaryObjectId)
                                                 //File
        [AuditedTrail]
        public Guid? DriverICBack { get; set; } //File, (BinaryObjectId)
                                                //File
        [AuditedTrail]
        public Guid? DriverLicenseFront { get; set; } //File, (BinaryObjectId)

        [AuditedTrail]
        public Guid? DriverLicenseBack { get; set; } //File, (BinaryObjectId)
        //File

        [AuditedTrail]
        public Guid? DriverEmploymentDetail { get; set; } //File, (BinaryObjectId)

        [AuditedTrail]
        public Guid? DriverHospitalDetail { get; set; } //File, (BinaryObjectId)

        [AuditedTrail]
        public Guid? DriverCarGrant { get; set; } //File, (BinaryObjectId)

        public int? RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public int? HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital HospitalFk { get; set; }

        [AuditedTrail]
        public int? CountryLocationId { get; set; }

        [ForeignKey("CountryLocationId")]
        public Location CountryLocationFk { get; set; }

        [AuditedTrail]
        public int? StateLocationId { get; set; }

        [ForeignKey("StateLocationId")]
        public Location StateLocationFk { get; set; }
        public long? OrganizationUnitId { get; set; }


    }
}