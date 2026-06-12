using System.Collections.Generic;
using ThinknInsurTech.Auditing.Dto;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
