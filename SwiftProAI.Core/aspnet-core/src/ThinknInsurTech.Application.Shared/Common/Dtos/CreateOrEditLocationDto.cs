using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class CreateOrEditLocationDto : EntityDto<int?>
    {

        public string ShortName { get; set; }

        public string Name { get; set; }

        public int ParentLocationId { get; set; }

    }
}