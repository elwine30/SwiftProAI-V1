using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.OCR.Dtos
{
    public class GetPromptForEditOutput
    {
        public CreateOrEditPromptDto Prompt { get; set; }

    }
}