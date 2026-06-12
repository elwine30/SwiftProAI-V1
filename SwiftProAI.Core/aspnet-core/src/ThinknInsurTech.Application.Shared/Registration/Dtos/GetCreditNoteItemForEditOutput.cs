using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCreditNoteItemForEditOutput
    {
        public CreateOrEditCreditNoteItemDto CreditNoteItem { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

    }
}