using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Companies.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}