using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllLocationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ShortNameFilter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxParentLocationIdFilter { get; set; }
        public int? MinParentLocationIdFilter { get; set; }

    }
}