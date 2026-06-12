using Abp.Application.Services.Dto;

namespace ThinknInsurTech.LawFirms.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}