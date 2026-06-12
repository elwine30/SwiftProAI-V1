using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Vehicles.Dtos
{
    public class CreateOrEditVehicleDto : EntityDto<int?>
    {

        public string Make { get; set; }

        public string Model { get; set; }

        public string Specification { get; set; }

    }
}