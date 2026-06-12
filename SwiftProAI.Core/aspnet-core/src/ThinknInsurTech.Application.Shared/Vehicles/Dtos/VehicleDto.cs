using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Vehicles.Dtos
{
    public class VehicleDto : EntityDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public string Specification { get; set; }

    }
}