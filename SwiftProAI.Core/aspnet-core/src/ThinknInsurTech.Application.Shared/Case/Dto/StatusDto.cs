using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Case.Dto
{
    public class StatusDto : EntityDto
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string Closeflag { get; set; }

        public string Type { get; set; }

    }
}


