using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("InvoiceItems")]
    public class InvoiceItem : FullAuditedEntity, IMustHaveTenant , IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(InvoiceItemConsts.MaxItemTypeLength, MinimumLength = InvoiceItemConsts.MinItemTypeLength)]
        public string ItemType { get; set; }

        [Required]
        [StringLength(InvoiceItemConsts.MaxRemarkLength, MinimumLength = InvoiceItemConsts.MinRemarkLength)]
        public string Remark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        [Precision(10,2)]
        public decimal Amount { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public long? OrganizationUnitId { get; set; }


    }
}