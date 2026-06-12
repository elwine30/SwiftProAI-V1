using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditHospitalDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string Address { get; set; }

        public int? CountryLocationId { get; set; }

        public int? StateLocationId { get; set; }

    }
}