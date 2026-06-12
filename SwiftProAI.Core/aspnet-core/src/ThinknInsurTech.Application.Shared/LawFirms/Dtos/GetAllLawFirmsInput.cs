using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.LawFirms.Dtos
{
    public class GetAllLawFirmsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ShortNameFilter { get; set; }

        public string AddressFilter { get; set; }

        public int? IsActiveFilter { get; set; }
    }
}