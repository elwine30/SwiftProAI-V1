using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Vehicles.Dtos
{
    public class GetVehicleForEditOutput
    {
        public CreateOrEditVehicleDto Vehicle { get; set; }

    }
}