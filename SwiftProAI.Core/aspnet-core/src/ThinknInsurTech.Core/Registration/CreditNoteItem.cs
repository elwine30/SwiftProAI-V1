using ThinknInsurTech.Registration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CreditNoteItems")]
    public class CreditNoteItem : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(CreditNoteItemConsts.MaxItemTypeLength, MinimumLength = CreditNoteItemConsts.MinItemTypeLength)]
        public string ItemType { get; set; }

        [Required]
        [StringLength(CreditNoteItemConsts.MaxRemarkLength, MinimumLength = CreditNoteItemConsts.MinRemarkLength)]
        public string Remark { get; set; }

        public decimal Amount { get; set; }

        public int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }
        public long? OrganizationUnitId { get; set; }


    }
}