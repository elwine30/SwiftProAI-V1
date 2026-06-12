using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Branches.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}