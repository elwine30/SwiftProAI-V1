using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseInsurerDto : EntityDto
    {
        public string ReferenceNo { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public int RegisterId { get; set; }

        public int? CompanyId { get; set; }

    }
}