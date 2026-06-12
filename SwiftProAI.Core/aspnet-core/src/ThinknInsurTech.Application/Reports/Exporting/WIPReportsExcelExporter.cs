using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Reports.Exporting
{
    public class WIPReportsExcelExporter : MiniExcelExcelExporterBase, IWIPReportsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WIPReportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWIPReportForViewDto> wipReports)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var wipReport in wipReports)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        //(Header, Data)
                        {L("ReportDate"), wipReport.WIPReport.ReportDate},
                        {L("VehicleNo"), wipReport.WIPReport.VehicleNo},
                        {L("CaseNo"), wipReport.WIPReport.CaseNo},
                        {L("InsuranceCompany"), wipReport.WIPReport.InsuranceCompany},
                        {L("CaseType"), wipReport.WIPReport.CaseType},
                        {L("AdjusterID"), wipReport.WIPReport.AdjusterName},
                        {L("InsurerRef"), wipReport.WIPReport.InsurerRef},
                        {L("LawyerRef"), wipReport.WIPReport.LawyerRef},
                        {L("CaseStatus"), wipReport.WIPReport.CaseStatus},
                        {L("AgingDays"), wipReport.WIPReport.AgingDays},
                        {L("DueDate"), wipReport.WIPReport.DueDate},

                    });
            }

            return CreateExcelPackage("WIPReportsList.xlsx", items);

        }
    }
}