using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Companies.Dto
{
    public class CompanyDto : EntityDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public int CaseTypeId { get; set; }

        public double ClaimRate { get; set; }

        public string Address { get; set; }

        public string GstRegNo { get; set; }

        public Boolean IsActive { get; set; }

        public double PhotographCharge { get; set; }

    }

}
