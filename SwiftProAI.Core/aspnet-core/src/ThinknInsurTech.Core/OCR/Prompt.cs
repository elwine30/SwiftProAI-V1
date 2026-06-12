using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.OCR
{
    [Table("Prompts")]
    [Auditable]
    public class Prompt : FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        [Required]
        [AuditedTrail]
        public string PromptName { get; set; }
        [AuditedTrail]
        public string PromptRequest { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}