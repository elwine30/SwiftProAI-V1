using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseSearchFeesInput : PagedAndSortedResultRequestDto
    {
        public string RegisterIdFilter { get; set; }

    }
}