using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class CreateOrEditLookupDto : EntityDto<int?>
    {

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Active { get; set; }

        public int Sequence { get; set; }

        [Required]
        public string Group { get; set; }

    }
}