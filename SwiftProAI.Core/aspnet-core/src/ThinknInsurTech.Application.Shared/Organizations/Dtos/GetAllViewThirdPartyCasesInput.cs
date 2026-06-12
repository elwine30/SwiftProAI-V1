using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Organizations.Dtos
{
    public class GetAllViewThirdPartyCasesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}