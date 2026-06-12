using System.Collections.Generic;

namespace ThinknInsurTech.Audit
{
    public class AuditTrail : AuditTrailBase
    {
        //Add any additional properties or methods here. They will not be overwritten by the code generator and will be preserved on re-generation.
        public virtual IEnumerable<AuditEntry> Changes { get; set; }
    }
}