using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.LawFirms.Dtos
{
    public class GetLawFirmForEditOutput
    {
        public CreateOrEditLawFirmDto LawFirm { get; set; }

    }
}