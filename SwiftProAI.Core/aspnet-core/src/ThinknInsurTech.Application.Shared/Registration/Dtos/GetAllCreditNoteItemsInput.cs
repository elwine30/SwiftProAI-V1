using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCreditNoteItemsInput : PagedAndSortedResultRequestDto
    {
        public string RegisterIdFilter { get; set; }

        public string ItemTypeFilter { get; set; }

    }
}