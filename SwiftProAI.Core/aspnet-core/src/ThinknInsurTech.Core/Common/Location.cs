using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Common
{
    [Table("Locations")]
    [Auditable]
    public class Location : Entity
    {

        [AuditedTrail]
        public virtual string ShortName { get; set; }

        [AuditedTrail]
        public virtual string Name { get; set; }

        [AuditedTrail]
        public virtual int ParentLocationId { get; set; }

    }
}