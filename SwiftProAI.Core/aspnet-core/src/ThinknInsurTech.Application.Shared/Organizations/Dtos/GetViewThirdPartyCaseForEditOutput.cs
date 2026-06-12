using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Organizations.Dtos
{
    public class GetViewThirdPartyCasesForEditOutput
    {
        public CreateOrEditViewThirdPartyCaseDto MainRegistrationOrganizationUnit { get; set; }

    }
}