using System.Collections.Generic;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Reports.Exporting
{
    public interface IWIPReportsExcelExporter
    {
        FileDto ExportToFile(List<GetWIPReportForViewDto> wipReports);
    }
}