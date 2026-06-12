using System.Collections.Generic;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Reports.Dto;

namespace ThinknInsurTech.Reports.Exporting
{
    public interface IWIPSummaryReportsExcelExporter
    {
        FileDto ExportWIPSummaryToFile(GetAllWipSummaryDataDto reportData, WipSummaryDataInputDto appliedFilter);
    }
}