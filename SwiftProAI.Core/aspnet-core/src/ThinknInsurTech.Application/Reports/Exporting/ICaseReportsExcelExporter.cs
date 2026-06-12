using System.Collections.Generic;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Reports.Exporting
{
    public interface ICaseReportsExcelExporter
    {
        FileDto ExportCaseReportToFile(GetCaseReportForViewDto caseReports, GetAllCaseReportsForExcelInput input);
    }
}