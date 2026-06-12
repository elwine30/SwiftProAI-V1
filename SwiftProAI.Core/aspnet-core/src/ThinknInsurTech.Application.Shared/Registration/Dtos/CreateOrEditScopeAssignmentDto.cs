using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditScopeAssignmentDto : EntityDto<int?>
    {

        [Required]
        public string Description { get; set; }

        public bool isActive { get; set; }

    }
}