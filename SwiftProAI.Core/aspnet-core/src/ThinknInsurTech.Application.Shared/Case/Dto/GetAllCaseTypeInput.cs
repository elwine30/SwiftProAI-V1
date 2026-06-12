
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Case.Dto
{
    public class GetAllCaseTypeInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ShortNameFilter { get; set; }

        public int? IsActiveFilter { get; set; }
    }
}