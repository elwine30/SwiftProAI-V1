using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinknInsurTech.Common
{
    [Table("Folders")]
    public class Folder : FullAuditedEntity, IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        public string MainEntity { get; set; }

        public string Field { get; set; }

        public int? MainEntityId { get; set; } // New property
        public long? OrganizationUnitId { get; set; }


    }
}