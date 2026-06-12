using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Case.Dto
{
    public class CaseTypeDto : EntityDto
    {
        public string Description { get; set; }

        public string ShortName { get; set; }

        public Boolean IsActive { get; set; }

    }
}
