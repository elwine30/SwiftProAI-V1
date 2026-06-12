using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCasePoliceReportsInput : PagedAndSortedResultRequestDto
    {

        public string RegisterIdFilter { get; set; }

    }
}