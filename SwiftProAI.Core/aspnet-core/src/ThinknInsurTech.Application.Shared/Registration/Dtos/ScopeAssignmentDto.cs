using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class ScopeAssignmentDto : EntityDto
    {
        public string Description { get; set; }

        public bool isActive { get; set; }

    }
}