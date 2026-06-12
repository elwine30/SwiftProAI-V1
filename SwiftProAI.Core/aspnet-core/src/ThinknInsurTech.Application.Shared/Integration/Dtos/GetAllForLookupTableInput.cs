using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Integration.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}