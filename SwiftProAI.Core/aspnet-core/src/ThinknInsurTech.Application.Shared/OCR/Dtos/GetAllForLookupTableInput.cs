using Abp.Application.Services.Dto;

namespace ThinknInsurTech.OCR.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}