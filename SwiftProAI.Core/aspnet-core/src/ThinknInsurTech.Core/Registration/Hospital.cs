using ThinknInsurTech.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Registration
{
    [Table("Hospitals")]
    [Auditable]
    public class Hospital : FullAuditedEntity
    {

        [AuditedTrail]
        public virtual string Name { get; set; }

        [AuditedTrail]
        public virtual string Address { get; set; }

        [AuditedTrail]
        public virtual int? CountryLocationId { get; set; }

        [ForeignKey("CountryLocationId")]
        public Location CountryLocationFk { get; set; }

        [AuditedTrail]
        public virtual int? StateLocationId { get; set; }

        [ForeignKey("StateLocationId")]
        public Location StateLocationFk { get; set; }

    }
}