using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;

namespace ThinknInsurTech.Vehicles
{
    [Table("Vehicles")]
    [Auditable]
    public class Vehicle : FullAuditedEntity
    {
        [AuditedTrail]
        public  string Make { get; set; }
        [AuditedTrail]
        public  string Model { get; set; }
        [AuditedTrail]
        public  string Specification { get; set; }

    }
}