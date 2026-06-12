using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Reports.Dto
{
    public class WipSummaryDataInputDto :EntityDto
    {
        public int? CaseTypeId { get; set; }
        public long? UserId { get; set; }
        public int? CompanyId { get; set; }

    }
}
