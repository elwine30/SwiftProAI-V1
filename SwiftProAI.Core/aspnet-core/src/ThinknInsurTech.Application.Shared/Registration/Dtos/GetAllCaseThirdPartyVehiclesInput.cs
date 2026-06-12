using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseThirdPartyVehiclesInput : PagedAndSortedResultRequestDto
    {

        public string RegisterIdFilter { get; set; }


    }
}