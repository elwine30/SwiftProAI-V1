using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}