using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseAdjustersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string StatusFilter { get; set; }

        public string ScopeAssignmentDescriptionFilter { get; set; }

        public string MainRegistrationVehicleNoFilter { get; set; }

        public string LocationNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}