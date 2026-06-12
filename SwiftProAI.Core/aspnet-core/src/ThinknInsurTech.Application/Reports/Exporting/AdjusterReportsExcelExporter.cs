using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Reports.Exporting
{
    public class AdjusterReportsExcelExporter : MiniExcelExcelExporterBase, IAdjusterReportsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AdjusterReportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAdjusterReportForViewDto> adjusterReports)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var adjusterReport in adjusterReports)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Case No"), adjusterReport.CaseNo},
                        {L("Created Date"), adjusterReport.CreatedDate},
                        {L("Insurance Company"), adjusterReport.InsuranceCompany},
                        {L("Insurance Case Ref."), adjusterReport.InsuranceCaseRef},
                        {L("Case Type"), adjusterReport.CaseType},
                        {L("Vehicle Number"), adjusterReport.VehicleNo},
                        {L("Service Fee"), adjusterReport.ServiceFee.ToString("F2")}, 
                    });
            }

            return CreateExcelPackage("AdjusterReportsList.xlsx", items);

        }
    }
}