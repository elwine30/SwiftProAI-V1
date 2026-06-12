using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Reports.Dto
{
    public class AdjusterReportMonthYearDto
    {
        public DateTime minMonthYear {  get; set; }
        public DateTime maxMonthYear { get; set; }
    }
}