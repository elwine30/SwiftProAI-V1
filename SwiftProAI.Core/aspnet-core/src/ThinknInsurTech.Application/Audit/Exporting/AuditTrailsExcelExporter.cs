using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Audit.Exporting
{
    public class AuditTrailsExcelExporter : MiniExcelExcelExporterBase, IAuditTrailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuditTrailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAuditTrailForViewDto> auditTrails)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var auditTrail in auditTrails)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Operation"), auditTrail.AuditTrail.Operation},
                        {L("TableName"), auditTrail.AuditTrail.TableName},
                        {L("ChangedBy"), auditTrail.AuditTrail.ChangedBy},
                        {L("ChangedDate"), auditTrail.AuditTrail.ChangedDate},

                    });
            }

            return CreateExcelPackage("AuditTrailsList.xlsx", items);

        }
    }
}