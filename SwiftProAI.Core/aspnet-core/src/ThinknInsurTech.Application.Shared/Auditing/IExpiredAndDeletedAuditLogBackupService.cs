using System.Collections.Generic;
using Abp.Auditing;

namespace ThinknInsurTech.Auditing
{
    public interface IExpiredAndDeletedAuditLogBackupService
    {
        bool CanBackup();
        
        void Backup(List<AuditLog> auditLogs);
    }
}