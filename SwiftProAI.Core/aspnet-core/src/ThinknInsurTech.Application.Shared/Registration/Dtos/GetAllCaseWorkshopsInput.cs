using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseWorkshopsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string EmailFilter { get; set; }

        public string ContactNoFilter { get; set; }

        public string ContactNameFilter { get; set; }

        public string MainRegistrationVehicleNoFilter { get; set; }

        public string WorkshopWorkshopNameFilter { get; set; }

    }
}