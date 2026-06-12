using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Case
{
    [Table("CaseType")]
    [Auditable]
    public class CaseType : FullAuditedEntity
    {
        [AuditedTrail]
        public string Description { get; set; }

        [AuditedTrail]
        public string ShortName { get; set; }

        [AuditedTrail]
        public Boolean IsActive { get; set; }

    }

}
