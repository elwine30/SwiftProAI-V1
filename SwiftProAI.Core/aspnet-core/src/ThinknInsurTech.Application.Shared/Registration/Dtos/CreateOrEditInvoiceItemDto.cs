using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditInvoiceItemDto : EntityDto<int?>
    {

        [Required]
        [StringLength(InvoiceItemConsts.MaxItemTypeLength, MinimumLength = InvoiceItemConsts.MinItemTypeLength)]
        public string ItemType { get; set; }

        [Required]
        [StringLength(InvoiceItemConsts.MaxRemarkLength, MinimumLength = InvoiceItemConsts.MinRemarkLength)]
        public string Remark { get; set; }

        public decimal Amount { get; set; }

        public int RegisterId { get; set; }

    }
}