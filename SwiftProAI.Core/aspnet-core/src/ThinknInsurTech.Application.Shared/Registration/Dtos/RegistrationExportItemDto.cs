using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class RegistrationExportItemDto
    {
        public MainRegistrationDocData MainRegistration { get; set; }
        public InsurersExportData Insurers { get; set; }
        public InsuredPersonOwnerDocData InsuredPersonOwner { get; set; }
        public IncidentDetailsDocData IncidentDetails { get; set; }
        public InsuredPersonDriverDocData InsuredPersonDriver { get; set; }
        public List<ThirdPartyVehicle> ThirdPartyVehicles { get; set; }
        public CasePoliceReportDocItem CasePoliceReport { get; set; }
    }

    public class MainRegistrationDocData
    {
        public int Id { get; set; }
        public string VehicleNo { get; set; }
    }

    public class InsurersExportData
    {
        public string Name { get; set; }
        public string ReferenceNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
    }

    public class InsuredPersonOwnerDocData
    {
        public string PolicyNo { get; set; }
        public string Coverage { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public bool isOwner { get; set; }
        public bool isDriver { get; set; }
        public string Vehicle { get; set; }
        public string NRIC { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Occupation { get; set; }
        public string EmployerName { get; set; }
        public Guid? ICFront { get; set; }
        public Guid? ICBack { get; set; }
        public Guid? DriverLicenseFront { get; set; }
        public Guid? DriverLicenseBack { get; set; }
    }

    public class InsuredPersonDriverDocData
    {
        public string Name { get; set; }
        public string Relationship { get; set; }
        public bool isOwner { get; set; }
        public bool isDriver { get; set; }
        public string Vehicle { get; set; }
        public string NRIC { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Occupation { get; set; }
        public string EmployerName { get; set; }
        public string Coverage { get; set; }
        public DateTime? LicenseDateFrom { get; set; }
        public DateTime? LicenseDateTo { get; set; }
        public string LicenseNo { get; set; }
        public string DrivingExp { get; set; }
        public string EmployerAddress { get; set; }
        public decimal MonthlyIncome { get; set; }
        public Guid? ICFront { get; set; }
        public Guid? ICBack { get; set; }
        public Guid? DriverLicenseFront { get; set; }
        public Guid? DriverLicenseBack { get; set; }
    }

    public class IncidentDetailsDocData
    {
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        public string VehicleNo { get; set; }
        public string DirectionTo { get; set; }
        public string DirectionFrom { get; set; }
        public string DrivingWith { get; set; }
        public string SpeedPrior { get; set; }
        public string SiteOfAccident { get; set; }
        public string TypeOfRoad { get; set; }
        public string WidthOfRoad { get; set; }
        public string CenterDemarcation { get; set; }
        public string DriverPathRight { get; set; }
        public string DriverPathLeft { get; set; }
        public string ViewOfRoad { get; set; }
        public string Visibility { get; set; }
        public string SurroundingArea { get; set; }
        public string RoadCondition { get; set; }
        public string WeatherCondition { get; set; }
    }

    public class ThirdPartyVehicle
    {
        public string VehicleNo { get; set; }
        public string RegisteredOwner { get; set; }
    }

    public class CasePoliceReportDocItem
    {
        public string IPD { get; set; }
        public string ReportNo { get; set; }
        public DateTime? ReportDate { get; set; }
        public string PoliceFindings { get; set; }
        public string PoliceOutcome { get; set; }
        public string OfficerName { get; set; }
        public string OfficerContact { get; set; }
        public string ServiceNo { get; set; }
    }
}
