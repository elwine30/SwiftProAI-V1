using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}