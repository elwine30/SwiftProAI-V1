using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class InsuredPersonDto : EntityDto
    {
        public bool IsOwner { get; set; }

        public bool IsDriver { get; set; }

        public bool IsThirdParty { get; set; }

        public string Relationship { get; set; }

        public string Name { get; set; }

        public string IdenticationType { get; set; }

        public string IdenticationNo { get; set; }

        public string Contact { get; set; }

        public string Nationality { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Specs { get; set; }

        public short Year { get; set; }

        public double Valuation { get; set; }

        public decimal ValuationEquiry { get; set; }

        public string PolicyNo { get; set; }

        public string Coverage { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string JpjRegisterNo { get; set; }

        public DateTime? JpjRegisterDate { get; set; }

        public string Occupation { get; set; }

        public string EmployerName { get; set; }

        public string EmployerContact { get; set; }

        public string EmployerAddress { get; set; }

        public double MonthlyIncome { get; set; }

        public string LicenseClasses { get; set; }

        public string LicenseNo { get; set; }

        public string DrivingExperience { get; set; }

        public DateTime? LicenseDateFrom { get; set; }

        public DateTime? LicenseDateTo { get; set; }

        public Guid? DriverICFront { get; set; }

        public string DriverICFrontFileName { get; set; }

        public Guid? DriverICBack { get; set; }

        public string DriverICBackFileName { get; set; }

        public Guid? DriverLicenseFront { get; set; }

        public string DriverLicenseFrontFileName { get; set; }

        public Guid? DriverLicenseBack { get; set; }

        public string DriverLicenseBackFileName { get; set; }

        public Guid? DriverEmploymentDetail { get; set; }

        public string DriverEmploymentDetailFileName { get; set; }

        public Guid? DriverHospitalDetail { get; set; }

        public string DriverHospitalDetailFileName { get; set; }

        public Guid? DriverCarGrant { get; set; }

        public string DriverCarGrantFileName { get; set; }

        public int? RegisterId { get; set; }

        public int HospitalId { get; set; }

        public int CountryLocationId { get; set; }

        public int StateLocationId { get; set; }

    }
}