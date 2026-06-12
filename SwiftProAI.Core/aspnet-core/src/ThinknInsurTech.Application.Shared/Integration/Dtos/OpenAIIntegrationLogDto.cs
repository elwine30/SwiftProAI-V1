using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Integration.Dtos
{
    public class OpenAIIntegrationLogDto : EntityDto
    {
        public string ActionUrl { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public int PromptToken { get; set; }

        public int CompletionToken { get; set; }

        public decimal TotalCost { get; set; }

        public string CaseNo { get; set; }

    }
}