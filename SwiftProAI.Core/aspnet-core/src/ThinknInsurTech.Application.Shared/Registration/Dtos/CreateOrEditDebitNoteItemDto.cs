using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditDebitNoteItemDto : EntityDto<int?>
    {

        [Required]
        [StringLength(DebitNoteItemConsts.MaxItemTypeLength, MinimumLength = DebitNoteItemConsts.MinItemTypeLength)]
        public string ItemType { get; set; }

        [Required]
        [StringLength(DebitNoteItemConsts.MaxRemarkLength, MinimumLength = DebitNoteItemConsts.MinRemarkLength)]
        public string Remark { get; set; }

        public decimal Amount { get; set; }

        public int RegisterId { get; set; }

    }
}