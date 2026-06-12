using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;
using System;

namespace ThinknInsurTech.Reports.Exporting
{
    public class CaseReportsExcelExporter : MiniExcelExcelExporterBase, ICaseReportsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CaseReportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        //public FileDto ExportToFile(List<GetCaseReportForViewDto> caseReports)
        //{

        //    var items = new List<Dictionary<string, object>>();

        //    foreach (var caseReport in caseReports)
        //    {
        //        items.Add(new Dictionary<string, object>()
        //            {
        //                {L("ReportFilter"), caseReport.CaseReport.ReportFilter},
        //                {L("ReportType"), caseReport.CaseReport.ReportType},
        //                {L("MonthRange"), caseReport.CaseReport.MonthRange},

        //            });
        //    }

        //    return CreateExcelPackage("CaseReportsList.xlsx", items);

        //}
        string GetFirstColumnHeader(GetAllCaseReportsForExcelInput appliedFilter)
        {
            switch (appliedFilter.ReportFilter)
            {
                case "insuranceCompany":
                    return "Insurance Company";
                case "caseType":
                    return "Case Type";
                case "state":
                    return "State";
                default:
                    return "";
            }
        }
        public FileDto ExportCaseReportToFile(GetCaseReportForViewDto reportDatas, GetAllCaseReportsForExcelInput appliedFilter)
        {
            string firstColumnHeader = GetFirstColumnHeader(appliedFilter);
            var items = new List<Dictionary<string, object>>();
            var formattedData = new Dictionary<string, Dictionary<string, int>>();

            foreach (var rowHeader in reportDatas.RowHeader)
            {
                formattedData[rowHeader] = new Dictionary<string, int>();
                foreach (var columnHeader in reportDatas.ColumnHeader)
                {
                    // Skip the first column as it is dynamic and treated as a header, not a data column
                    if (columnHeader != firstColumnHeader)
                    {
                        formattedData[rowHeader][columnHeader] = 0; // Default value if not present
                    }
                }
            }

            // Populate formattedData with actual values from reportDatas.ReportData
            foreach (var rowHeader in reportDatas.RowHeader)
            {
                if (reportDatas.ReportData.ContainsKey(rowHeader))
                {
                    foreach (var kvp in reportDatas.ReportData[rowHeader])
                    {
                        // Make sure the column exists in the header to avoid any key not found issues
                        if (formattedData[rowHeader].ContainsKey(kvp.Key))
                        {
                            formattedData[rowHeader][kvp.Key] = kvp.Value;
                        }
                    }
                }
            }

            // Create items for export
            foreach (var rowHeader in reportDatas.RowHeader)
            {
                var item = new Dictionary<string, object> { { firstColumnHeader, rowHeader } };

                foreach (var columnHeader in reportDatas.ColumnHeader)
                {
                    // Skip the first column as it is used as a key in the dictionary
                    if (columnHeader != firstColumnHeader)
                    {
                        item[columnHeader] = formattedData[rowHeader][columnHeader];
                    }
                }

                items.Add(item);
            }

            return CreateExcelPackage("CaseReportsList.xlsx", items);

        }

    }
}