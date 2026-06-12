using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCasePoliceReportDto : EntityDto<int?>
    {

        [StringLength(CasePoliceReportConsts.MaxIPDLength, MinimumLength = CasePoliceReportConsts.MinIPDLength)]
        public string IPD { get; set; }

        [StringLength(CasePoliceReportConsts.MaxPoliceStationLength, MinimumLength = CasePoliceReportConsts.MinPoliceStationLength)]
        public string PoliceStation { get; set; }

        [StringLength(CasePoliceReportConsts.MaxVehicleNoLength, MinimumLength = CasePoliceReportConsts.MinVehicleNoLength)]
        public string VehicleNo { get; set; }

        [StringLength(CasePoliceReportConsts.MaxReportNoLength, MinimumLength = CasePoliceReportConsts.MinReportNoLength)]
        public string ReportNo { get; set; }

        public DateTime ReportTime { get; set; }

        public DateTime IncidentTime { get; set; }

        public string LateReport { get; set; }

        public string LateReason { get; set; }

        [StringLength(CasePoliceReportConsts.MaxTypeLength, MinimumLength = CasePoliceReportConsts.MinTypeLength)]
        public string Type { get; set; }

        public string OfficerName { get; set; }

        [StringLength(CasePoliceReportConsts.MaxContactLength, MinimumLength = CasePoliceReportConsts.MinContactLength, ErrorMessage = "Office Contact Number must be between 8 and 12 digits")]
        public string OfficerContact { get; set; }
        public string ServiceNo { get; set; }

        public string PoliceFinding { get; set; }   

        public string PoliceOutcome { get; set; }

        public Guid? ReportFileUpload { get; set; }

        public string ReportFileUploadToken { get; set; }

        public int RegisterId { get; set; }

        [Required]
        public string ReportType { get; set; }

        public string ArmedForceId { get; set; }

        public string PassportNo { get; set; }

        public string Statement { get; set; }

        public string ComplainantIdentityNo { get; set; }
    }
}