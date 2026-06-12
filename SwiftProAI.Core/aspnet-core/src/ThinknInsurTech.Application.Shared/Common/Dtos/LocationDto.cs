using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class LocationDto : EntityDto
    {
        public string ShortName { get; set; }

        public string Name { get; set; }

        public int ParentLocationId { get; set; }

    }
}