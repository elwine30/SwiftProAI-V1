using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class LookupDto : EntityDto
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public int Sequence { get; set; }

        public string Group { get; set; }

    }
}