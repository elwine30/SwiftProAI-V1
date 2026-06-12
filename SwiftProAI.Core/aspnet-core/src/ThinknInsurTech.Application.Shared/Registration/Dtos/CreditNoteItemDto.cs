using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreditNoteItemDto : EntityDto
    {
        public string ItemType { get; set; }

        public string Remark { get; set; }

        public decimal Amount { get; set; }

        public int RegisterId { get; set; }

    }
}