using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Organizations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}