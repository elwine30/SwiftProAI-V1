using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllStaffsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NRICFilter { get; set; }

        public string AddressFilter { get; set; }

        public string PassportFilter { get; set; }

        public decimal? MaxServiceFeePercentFilter { get; set; }
        public decimal? MinServiceFeePercentFilter { get; set; }

        public decimal? MaxFraudFeePercentFilter { get; set; }
        public decimal? MinFraudFeePercentFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string GroupNameFilter { get; set; }

    }
}