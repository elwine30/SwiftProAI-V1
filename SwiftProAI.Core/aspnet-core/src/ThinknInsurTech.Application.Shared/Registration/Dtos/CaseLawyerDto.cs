using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseLawyerDto : EntityDto
    {
        public DateTime HearingDate { get; set; }

        public string ReferenceNo { get; set; }

        public string ContactNo { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Type { get; set; }

        public int RegisterId { get; set; }

        public int LawFirmId { get; set; }

    }
}