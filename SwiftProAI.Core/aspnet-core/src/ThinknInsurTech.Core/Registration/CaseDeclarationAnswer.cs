
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseDeclarationAnswers")]
    [Auditable]
    public class CaseDeclarationAnswer : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [AuditedTrail]
        public string Answer { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public virtual int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public DeclarationQuestion QuestionFk { get; set; }

        public long? OrganizationUnitId { get; set; }



    }
}