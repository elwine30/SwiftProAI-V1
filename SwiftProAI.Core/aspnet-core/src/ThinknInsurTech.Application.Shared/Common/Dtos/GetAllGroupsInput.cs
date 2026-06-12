using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllGroupsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? GroupTypeFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string BranchNameFilter { get; set; }

    }
}