using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditDeclarationQuestionDto : EntityDto<int?>
    {

        public string Question { get; set; }

        public string OptionType { get; set; }

        public string OptionValues { get; set; }

    }
}