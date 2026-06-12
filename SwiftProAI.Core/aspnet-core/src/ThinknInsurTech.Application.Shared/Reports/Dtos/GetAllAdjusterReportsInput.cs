using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Reports.Dtos
{
    public class GetAllAdjusterReportsInput : PagedAndSortedResultRequestDto
    {
        public DateTime MonthFilter { get; set; }
        public DateTime YearFilter { get; set; }
        public int? UserIdFilter { get; set; }


    }
}