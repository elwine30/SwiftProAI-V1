using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class HospitalDto : EntityDto
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public int? CountryLocationId { get; set; }

        public int? StateLocationId { get; set; }

    }
}