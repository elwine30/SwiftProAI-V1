using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;
using ThinknInsurTech.Reports.Dto;
using System;
using System.IO;
using MiniExcelLibs;
using System.Linq;

namespace ThinknInsurTech.Reports.Exporting
{
    public class WIPSummaryReportsExcelExporter : MiniExcelExcelExporterBase, IWIPSummaryReportsExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WIPSummaryReportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        

        public FileDto ExportWIPSummaryToFile(GetAllWipSummaryDataDto reportDatas, WipSummaryDataInputDto appliedFilter)
        {
            var items = new List<Dictionary<string, object>>();

            var formattedData = new Dictionary<string, Dictionary<string, int>>();

            foreach (var rowHeader in reportDatas.RowHeaders)
            {
                formattedData[rowHeader] = new Dictionary<string, int>();
                foreach (var columnHeader in reportDatas.ColumnHeaders)
                {
                    formattedData[rowHeader][columnHeader] = 0; // Default value if not present
                }
            }

            foreach (var columnHeader in reportDatas.ColumnHeaders)
            {
                foreach (var rowHeader in reportDatas.RowHeaders)
                {
                    if (reportDatas.Data.ContainsKey(columnHeader) && reportDatas.Data[columnHeader].ContainsKey(rowHeader))
                    {
                        formattedData[rowHeader][columnHeader] = reportDatas.Data[columnHeader][rowHeader];
                    }
                }
            }

            foreach (var rowHeader in reportDatas.RowHeaders)
            {
                var item = new Dictionary<string, object>
                 {
                     { " ", rowHeader }
                 };

                foreach (var columnHeader in reportDatas.ColumnHeaders)
                {
                    item[columnHeader] = formattedData[rowHeader][columnHeader];
                }

                items.Add(item);
            }

            return CreateExcelPackage("WIPSummaryReportsList.xlsx", items);
        
        }

    }
}

#region comment

//public FileDto ExportToFile(List<GetWIPSummaryReportForViewDto> wipSummaryReports)
//{

//    var items = new List<Dictionary<string, object>>();

//    foreach (var wipSummaryReport in wipSummaryReports)
//    {
//        items.Add(new Dictionary<string, object>()
//        {

//        });
//    }

//    return CreateExcelPackage("WIPSummaryReportsList.xlsx", items);

//}
//public FileDto ExportWIPSummaryToFile(List<GetWIPSummaryReportForViewDto> wipSummaryReports)
//{
//    var items = new List<Dictionary<string, object>>();

//    foreach (var wipSummaryReport in wipSummaryReports)
//    {
//        var item = new Dictionary<string, object>
//    {
//        { "CaseTypeShortName", wipSummaryReport.CaseTypeShortName },
//        { "UserName", wipSummaryReport.UserName },
//        { "CompanyShortName", wipSummaryReport.CompanyShortName },
//        // Add other properties as needed
//    };

//        items.Add(item);
//    }

//    return CreateExcelPackage("WIPSummaryReportsList.xlsx", items);
//}




#endregion