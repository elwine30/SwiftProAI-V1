using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.OCR.Dtos
{
    public class CreateOrEditPromptDto : EntityDto<int?>
    {

        [Required]
        public string PromptName { get; set; }

        public string PromptRequest { get; set; }

    }
}