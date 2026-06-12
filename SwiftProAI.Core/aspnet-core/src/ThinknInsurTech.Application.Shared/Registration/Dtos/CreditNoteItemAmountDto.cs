using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreditNoteItemAmountDto : EntityDto
    {
        public string ItemType { get; set; }

        public decimal Amount { get; set; }

    }
}