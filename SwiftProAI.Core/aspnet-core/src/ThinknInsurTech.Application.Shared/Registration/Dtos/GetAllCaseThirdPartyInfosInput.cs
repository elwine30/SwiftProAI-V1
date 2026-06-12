using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseThirdPartyInfosInput : PagedAndSortedResultRequestDto
    {
        public int RegisterId { get; set; }

    }
}