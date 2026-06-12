using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Approval.Dtos
{
    public class GetViewThirdPartyCaseRequestForEditOutput
    {
        public CreateOrEditViewThirdPartyCaseRequestDto ViewThirdPartyCaseRequest { get; set; }

    }
}