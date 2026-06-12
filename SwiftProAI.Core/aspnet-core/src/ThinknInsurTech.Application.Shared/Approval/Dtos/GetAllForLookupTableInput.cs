using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Approval.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}