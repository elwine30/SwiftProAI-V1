using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCasePoliceReportSummaryForEditOutput
    {
        public CreateOrEditCasePoliceReportSummaryDto CasePoliceReportSummary { get; set; }

    }
}