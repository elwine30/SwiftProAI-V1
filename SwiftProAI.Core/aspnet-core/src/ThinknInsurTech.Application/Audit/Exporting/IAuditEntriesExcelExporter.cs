using System.Collections.Generic;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Audit.Exporting
{
    public interface IAuditEntriesExcelExporter
    {
        FileDto ExportToFile(List<GetAuditEntryForViewDto> auditEntries);
    }
}