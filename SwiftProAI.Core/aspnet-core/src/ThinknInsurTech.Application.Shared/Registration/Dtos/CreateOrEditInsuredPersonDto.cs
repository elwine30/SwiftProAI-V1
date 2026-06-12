using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditInsuredPersonDto : EntityDto<int?>
    {

        public bool IsOwner { get; set; }

        public bool IsDriver { get; set; }

        public bool IsThirdParty { get; set; }

        public string Relationship { get; set; }

        [Required]
        [StringLength(InsuredPersonConsts.MaxNameLength, MinimumLength = InsuredPersonConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        public string IdenticationType { get; set; }

        [Required]
        [StringLength(InsuredPersonConsts.MaxIdenticationNoLength, MinimumLength = InsuredPersonConsts.MinIdenticationNoLength)]
        public string IdenticationNo { get; set; }

        [StringLength(InsuredPersonConsts.MaxContactLength, MinimumLength = InsuredPersonConsts.MinContactLength, ErrorMessage = "Contact Number must be between 8 and 12 digits")]
        public string Contact { get; set; }

        [StringLength(InsuredPersonConsts.MaxNationalityLength, MinimumLength = InsuredPersonConsts.MinNationalityLength)]
        public string Nationality { get; set; }

        [StringLength(InsuredPersonConsts.MaxMakeLength, MinimumLength = InsuredPersonConsts.MinMakeLength)]
        public string Make { get; set; }

        [StringLength(InsuredPersonConsts.MaxModelLength, MinimumLength = InsuredPersonConsts.MinModelLength)]
        public string Model { get; set; }

        [StringLength(InsuredPersonConsts.MaxSpecsLength, MinimumLength = InsuredPersonConsts.MinSpecsLength)]
        public string Specs { get; set; }

        public short? Year { get; set; }

        public double? Valuation { get; set; }

        public decimal? ValuationEquiry { get; set; }

        [StringLength(InsuredPersonConsts.MaxPolicyNoLength, MinimumLength = InsuredPersonConsts.MinPolicyNoLength)]
        public string PolicyNo { get; set; }

        [StringLength(InsuredPersonConsts.MaxCoverageLength, MinimumLength = InsuredPersonConsts.MinCoverageLength)]
        public string Coverage { get; set; }

        [StringLength(InsuredPersonConsts.MaxPostcodeLength, MinimumLength = InsuredPersonConsts.MinPostcodeLength)]
        public string Postcode { get; set; }

        [StringLength(InsuredPersonConsts.MaxCityLength, MinimumLength = InsuredPersonConsts.MinCityLength)]
        public string City { get; set; }

        [StringLength(InsuredPersonConsts.MaxAddressLength, MinimumLength = InsuredPersonConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(InsuredPersonConsts.MaxJpjRegisterNoLength, MinimumLength = InsuredPersonConsts.MinJpjRegisterNoLength)]
        public string JpjRegisterNo { get; set; }

        public DateTime? JpjRegisterDate { get; set; }

        [StringLength(InsuredPersonConsts.MaxOccupationLength, MinimumLength = InsuredPersonConsts.MinOccupationLength)]
        public string Occupation { get; set; }

        [StringLength(InsuredPersonConsts.MaxEmployerNameLength, MinimumLength = InsuredPersonConsts.MinEmployerNameLength)]
        public string EmployerName { get; set; }

        [StringLength(InsuredPersonConsts.MaxEmployerContactLength, MinimumLength = InsuredPersonConsts.MinEmployerContactLength, ErrorMessage = "Employer Contact Number must be between 8 and 12 digits")]
        public string EmployerContact { get; set; }

        [StringLength(InsuredPersonConsts.MaxEmployerAddressLength, MinimumLength = InsuredPersonConsts.MinEmployerAddressLength)]
        public string EmployerAddress { get; set; }

        public double MonthlyIncome { get; set; }

        [StringLength(InsuredPersonConsts.MaxLicenseClassesLength, MinimumLength = InsuredPersonConsts.MinLicenseClassesLength)]
        public string LicenseClasses { get; set; }

        [StringLength(InsuredPersonConsts.MaxLicenseNoLength, MinimumLength = InsuredPersonConsts.MinLicenseNoLength)]
        public string LicenseNo { get; set; }

        [StringLength(InsuredPersonConsts.MaxDrivingExperienceLength, MinimumLength = InsuredPersonConsts.MinDrivingExperienceLength)]
        public string DrivingExperience { get; set; }

        public DateTime? LicenseDateFrom { get; set; }

        public DateTime? LicenseDateTo { get; set; }

        public Guid? DriverICFront { get; set; }

        public string DriverICFrontToken { get; set; }

        public Guid? DriverICBack { get; set; }

        public string DriverICBackToken { get; set; }

        public Guid? DriverLicenseFront { get; set; }

        public string DriverLicenseFrontToken { get; set; }

        public Guid? DriverLicenseBack { get; set; }

        public string DriverLicenseBackToken { get; set; }

        public Guid? DriverEmploymentDetail { get; set; }

        public string DriverEmploymentDetailToken { get; set; }

        public Guid? DriverHospitalDetail { get; set; }

        public string DriverHospitalDetailToken { get; set; }

        public Guid? DriverCarGrant { get; set; }

        public string DriverCarGrantToken { get; set; }

        public int RegisterId { get; set; }

        public int? HospitalId { get; set; }

        public int? CountryLocationId { get; set; }

        public int? StateLocationId { get; set; }

    }
}