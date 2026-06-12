using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Companies.Dtos
{
    public class GetAllCompaniesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ShortNameFilter { get; set; }

        public decimal? MaxClaimRateFilter { get; set; }
        public decimal? MinClaimRateFilter { get; set; }

        public string AddressFilter { get; set; }

        public string GstRegNoFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public decimal? MaxPhotographChargeFilter { get; set; }
        public decimal? MinPhotographChargeFilter { get; set; }

        public string CaseTypeDescriptionFilter { get; set; }

    }
}