using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Common
{
    public enum EnumGroupType
    {
        Admin = 1,
        Adjuster = 2,
        Editor = 3,
        Finance = 4,
        Others = 5
    }

    public static class EnumFolderField
    {
        public const string
        AssignmentSheetDoc = "ASSG-SHEET-DOCUMENT",
        DriverDrivingLicenseBack = "DRI-DL-B",
        DriverDrivingLicenseFront = "DRI-DL-F",
        DriverEmploymentDoc = "DRI-EMP",
        DriverHospitalDoc = "DRI-HOSPITAL",
        DriverICBack = "DRI-NRIC-B",
        DriverICFront = "DRI-NRIC-F",
        IncidentDetailDoc = "INC-DET",
        InsuredOwnerDrivingLicenseBack = "INS-DL-B",
        InsuredOwnerDrivingLicenseFront = "INS-DL-F",
        InsuredOwnerEmploymentDoc = "INS-EMP",
        InsuredOwnerHospitalDoc = "INS-HOSPITAL",
        InsuredOwnerICBack = "INS-NRIC-B",
        InsuredOwnerICFront = "INS-NRIC-F",
        InsurerDetailDoc = "INSURER-DET",
        LawRepDoc = "LAW-REP",
        OtherRepDoc = "OTHER-REP",
        PoliceReportDriver = "POL-REP-DRI",
        PoliceReportInsurer = "POL-REP-INS",
        WorkshopRepDoc = "WORKSHOP-REP",
        DriverCarGrant = "DRI-CAR-GRANT",
        InsuredOwnerCarGrant = "INS-CAR-GRANT",
        ClaimantPoliceReport = "CLM-POL-REP",
        ThirdPartyPoliceReport = "TRP-POL-REP";
    }

}
