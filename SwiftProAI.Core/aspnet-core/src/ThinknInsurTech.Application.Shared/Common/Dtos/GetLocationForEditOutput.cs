using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetLocationForEditOutput
    {
        public CreateOrEditLocationDto Location { get; set; }

    }
}