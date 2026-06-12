using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllScopeAssignmentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? isActiveFilter { get; set; }

    }
}