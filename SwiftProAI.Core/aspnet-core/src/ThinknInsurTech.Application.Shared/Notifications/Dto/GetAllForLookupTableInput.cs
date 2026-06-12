using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}