using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class DebitNoteItemAmountDto : EntityDto
    {
        public string ItemType { get; set; }

        public decimal Amount { get; set; }

    }
}
