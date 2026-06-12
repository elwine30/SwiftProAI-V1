using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using ThinknInsurTech.Audit;


namespace ThinknInsurTech.Case
{
    [Table("Status")]
    [Auditable]
    public class Status : Entity<int>
    {
        [AuditedTrail]
        public string Code { get; set; }

        [AuditedTrail]
        public string Description { get; set; }

        [AuditedTrail]
        public string Closeflag { get; set; }

        [AuditedTrail]
        public string Type { get; set; }

    }
}
