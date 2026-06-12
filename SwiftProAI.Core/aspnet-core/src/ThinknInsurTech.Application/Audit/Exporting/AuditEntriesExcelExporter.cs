using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Audit.Exporting
{
    public class AuditEntriesExcelExporter : MiniExcelExcelExporterBase, IAuditEntriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuditEntriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAuditEntryForViewDto> auditEntries)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var auditEntry in auditEntries)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("FieldName"), auditEntry.AuditEntry.FieldName},
                        {L("OldValue"), auditEntry.AuditEntry.OldValue},
                        {L("NewValue"), auditEntry.AuditEntry.NewValue},

                    });
            }

            return CreateExcelPackage("AuditEntriesList.xlsx", items);

        }
    }
}