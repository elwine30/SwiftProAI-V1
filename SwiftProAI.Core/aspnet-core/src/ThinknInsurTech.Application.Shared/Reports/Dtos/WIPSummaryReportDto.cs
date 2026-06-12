using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Reports.Dtos
{
    public class WIPSummaryReportDto : EntityDto
    {
        public int? CaseTypeId { get; set; }
        public long? UserId { get; set; }
        public int? CompanyId { get; set; }

    }
}