using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Integration.Dtos
{
    public class GetAllOpenAIIntegrationLogsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ActionUrlFilter { get; set; }

        public string RequestFilter { get; set; }

        public string ResponseFilter { get; set; }

        public int? MaxPromptTokenFilter { get; set; }
        public int? MinPromptTokenFilter { get; set; }

        public int? MaxCompletionTokenFilter { get; set; }
        public int? MinCompletionTokenFilter { get; set; }

        public decimal? MaxTotalCostFilter { get; set; }
        public decimal? MinTotalCostFilter { get; set; }

        public string CaseNoFilter { get; set; }
        public long OUIdFilter { get; set; }
        public DateTime? MinDateFilter { get; set; }
        public DateTime? MaxDateFilter { get; set; }

    }
}