using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAllWipSummaryDataDto
    {
        public WIPSummaryReportDto AppliedFilter { get; set; }
        public List<String> ColumnHeaders { get; set; }
        public List<String> RowHeaders { get; set; }
        public Dictionary<string, Dictionary<string, int>> Data { get; set; }
    }

    
}

