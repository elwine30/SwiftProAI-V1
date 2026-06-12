using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllLookupsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public int? MaxSequenceFilter { get; set; }
        public int? MinSequenceFilter { get; set; }

        public string GroupFilter { get; set; }

    }
}