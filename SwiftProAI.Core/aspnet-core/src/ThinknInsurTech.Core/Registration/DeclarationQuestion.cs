using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("DeclarationQuestions")]
    [Auditable]
    public class DeclarationQuestion : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(DeclarationQuestionConsts.MaxQuestionLength, MinimumLength = DeclarationQuestionConsts.MinQuestionLength)]
        [AuditedTrail]
        public string Question { get; set; }

        [Required]
        [StringLength(DeclarationQuestionConsts.MaxOptionTypeLength, MinimumLength = DeclarationQuestionConsts.MinOptionTypeLength)]
        [AuditedTrail]
        public string OptionType { get; set; }

        [AuditedTrail]
        public string OptionValues { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}