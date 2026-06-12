using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCreditNoteItemDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CreditNoteItemConsts.MaxItemTypeLength, MinimumLength = CreditNoteItemConsts.MinItemTypeLength)]
        public string ItemType { get; set; }

        [Required]
        [StringLength(CreditNoteItemConsts.MaxRemarkLength, MinimumLength = CreditNoteItemConsts.MinRemarkLength)]
        public string Remark { get; set; }

        public decimal Amount { get; set; }

        public int RegisterId { get; set; }

    }
}