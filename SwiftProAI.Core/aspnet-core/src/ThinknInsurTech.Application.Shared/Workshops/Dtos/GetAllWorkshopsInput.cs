using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Workshops.Dtos
{
    public class GetAllWorkshopsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string WorkshopNoFilter { get; set; }

        public string WorkshopNameFilter { get; set; }

        public string AddressFilter { get; set; }

        public double? MaxClaimRateFilter { get; set; }
        public double? MinClaimRateFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}