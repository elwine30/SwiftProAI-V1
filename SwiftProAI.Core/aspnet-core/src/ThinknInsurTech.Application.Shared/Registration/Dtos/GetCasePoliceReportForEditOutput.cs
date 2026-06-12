using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCasePoliceReportForEditOutput
    {
        public CreateOrEditCasePoliceReportDto CasePoliceReport { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string ReportFileUploadFileName { get; set; }

        public DateTime? IncidentTimeTo { get; set; }

    }
}