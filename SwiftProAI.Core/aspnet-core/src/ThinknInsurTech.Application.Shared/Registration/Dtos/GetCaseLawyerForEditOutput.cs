using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseLawyerForEditOutput
    {
        public CreateOrEditCaseLawyerDto CaseLawyer { get; set; }

        public string LawFirmName { get; set; }

    }
}