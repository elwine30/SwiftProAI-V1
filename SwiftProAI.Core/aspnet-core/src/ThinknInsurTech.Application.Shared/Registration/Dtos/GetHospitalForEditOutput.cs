using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetHospitalForEditOutput
    {
        public CreateOrEditHospitalDto Hospital { get; set; }

        public string LocationName { get; set; }

        public string LocationName2 { get; set; }

    }
}