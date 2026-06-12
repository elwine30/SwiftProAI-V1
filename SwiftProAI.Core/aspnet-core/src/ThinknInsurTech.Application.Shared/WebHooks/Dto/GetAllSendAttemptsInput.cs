using ThinknInsurTech.Dto;

namespace ThinknInsurTech.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
