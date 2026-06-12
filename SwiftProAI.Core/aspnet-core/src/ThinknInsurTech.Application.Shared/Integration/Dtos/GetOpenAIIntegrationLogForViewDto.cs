using System;

namespace ThinknInsurTech.Integration.Dtos
{
    public class GetOpenAIIntegrationLogForViewDto
    {
        public OpenAIIntegrationLogDto OpenAIIntegrationLog { get; set; }

        public string OrganizationUnitName { get; set; }
        public long OrganizationUnitId { get; set; }
        public DateTime createdDate { get; set; }

    }
}