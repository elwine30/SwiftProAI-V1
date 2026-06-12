using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.OCR.Dtos
{
    public class GetAllPromptsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PromptNameFilter { get; set; }

        public string PromptRequestFilter { get; set; }

    }
}