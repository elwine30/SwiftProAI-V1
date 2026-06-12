using System.Collections.Generic;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetCaseReportForViewDto
    {
        public CaseReportDto CaseReport { get; set; }

        public List<string> RowHeader { get; set; }
        public List<string> ColumnHeader { get; set; }

        public Dictionary<string, Dictionary<string, int>> ReportData { get; set; }

    }
}