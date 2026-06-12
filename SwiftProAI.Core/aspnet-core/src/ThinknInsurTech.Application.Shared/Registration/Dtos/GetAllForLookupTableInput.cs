using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}