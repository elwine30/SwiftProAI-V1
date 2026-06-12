using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Organizations.Dtos
{
    public class CreateOrEditViewThirdPartyCaseDto : EntityDto<int?>
    {

        public int RegistrationId { get; set; }

        public long? AssignedOUId { get; set; }

    }
}