using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Remarks
{
    [Table("Remark")]
    [Auditable]
    public class Remark : CreationAuditedEntity<int>
    {
        [AuditedTrail]
        public int RegisterId { get; set; }

        [AuditedTrail]
        public string Description { get; set; }


    }
}
