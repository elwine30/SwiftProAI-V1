using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{

    public class InvoiceItemAmountDto : EntityDto
    {
        public string ItemType { get; set; }

        public decimal Amount { get; set; }

    }

}
