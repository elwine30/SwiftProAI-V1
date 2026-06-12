using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAllWIPReportsForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxReportDateFilter { get; set; }
        public DateTime? MinReportDateFilter { get; set; }

        public int? InsuranceCompanyFilter { get; set; }

        public int? LawyerCompanyIDFilter { get; set; }

        public int? AdjusterIDFilter { get; set; }

        public int? AdjusterGroupIDFilter { get; set; }

    }
}