using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllInvoiceItemsInput : PagedAndSortedResultRequestDto
    {
        public string RegisterIdFilter { get; set; }

        public string ItemTypeFilter { get; set; }


    }
}