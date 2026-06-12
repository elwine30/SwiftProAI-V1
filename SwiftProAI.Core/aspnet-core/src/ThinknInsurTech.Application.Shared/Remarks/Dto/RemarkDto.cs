using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Remarks.Dto
{
    public class RemarkDto : EntityDto
    {
        public int RegisterId { get; set; }

        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorUserName { get; set; }

    }
}
