using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllHospitalsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string AddressFilter { get; set; }

        public string LocationNameFilter { get; set; }

        public string LocationName2Filter { get; set; }

    }
}