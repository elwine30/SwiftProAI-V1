using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Organizations
{
    [Table("ViewThirdPartyCases")]
    public class ViewThirdPartyCases : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration MainRegistrationFk { get; set; }

        public virtual long? AssignedOUId { get; set; }

    }
}