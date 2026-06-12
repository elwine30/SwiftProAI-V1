using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Integration
{
    [Table("OpenAIIntegrationLogs")]
    public class OpenAIIntegrationLog : CreationAuditedEntity
    {

        [Required]
        public virtual string ActionUrl { get; set; }

        [Required]
        public virtual string Request { get; set; }

        public virtual string Response { get; set; }

        public virtual int PromptToken { get; set; }

        public virtual int CompletionToken { get; set; }

        public virtual decimal TotalCost { get; set; }

        public virtual string CaseNo { get; set; }

    }
}