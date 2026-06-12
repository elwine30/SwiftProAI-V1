using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseWorkshopDto : EntityDto
    {
        public string Email { get; set; }

        public string ContactNo { get; set; }

        public string ContactName { get; set; }

        public int RegisterId { get; set; }

        public int? WorkshopId { get; set; }

    }
}