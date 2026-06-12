using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CasePoliceReportDto : EntityDto
    {
        public string IPD { get; set; }

        public string PoliceStation { get; set; }

        public string VehicleNo { get; set; }

        public string ReportNo { get; set; }

        public DateTime ReportTime { get; set; }
        public DateTime IncidentTime { get; set; }

        public string LateReport { get; set; }

        public string LateReason { get; set; }
        public string OfficerName { get; set; }
        public string ServiceNo { get; set; }
        public string OfficerContact { get; set; }  

        public string Type { get; set; }

        public string PoliceFinding { get; set; }

        public string PoliceOutcome { get; set; }

        public Guid? ReportFileUpload { get; set; }

        public string ReportFileUploadFileName { get; set; }

        public int RegisterId { get; set; }

        public string ReportType { get; set; }

        public bool IsDataConsistent { get; set; }

        public string Statement { get; set; }

        public string ComplainantIdentityNo { get; set; }
    }
}