using Abp.Domain.Repositories;
using ThinknInsurTech.Audit.Exporting;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using ThinknInsurTech.Storage;
using System;

namespace ThinknInsurTech.Audit
{
    [AbpAuthorize(AppPermissions.Pages_Administration_AuditTrails)]
    public class AuditTrailsAppService : AuditTrailsAppServiceBase, IAuditTrailsAppService, IAuditTrailsAppServiceExtended
    {
        public AuditTrailsAppService(IRepository<AuditTrail> auditTrailRepository, IAuditTrailsExcelExporter auditTrailsExcelExporter)
        : base(auditTrailRepository, auditTrailsExcelExporter
        )
        {
        }

        // Write your custom code here. 
        // ASP.NET Zero Power Tools will not overwrite this class when you regenerate the related entity.
    }
}