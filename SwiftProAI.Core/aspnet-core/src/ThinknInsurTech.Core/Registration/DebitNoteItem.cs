using ThinknInsurTech.Registration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("DebitNoteItems")]
    public class DebitNoteItem : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(DebitNoteItemConsts.MaxItemTypeLength, MinimumLength = DebitNoteItemConsts.MinItemTypeLength)]
        public string ItemType { get; set; }

        [Required]
        [StringLength(DebitNoteItemConsts.MaxRemarkLength, MinimumLength = DebitNoteItemConsts.MinRemarkLength)]
        public string Remark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }
        public long? OrganizationUnitId { get; set; }


    }
}