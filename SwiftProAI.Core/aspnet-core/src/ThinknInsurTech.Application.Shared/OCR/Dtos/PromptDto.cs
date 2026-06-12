using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.OCR.Dtos
{
    public class PromptDto : EntityDto
    {
        public string PromptName { get; set; }

        public string PromptRequest { get; set; }

    }
}