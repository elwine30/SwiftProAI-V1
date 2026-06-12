using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseInsurerForEditOutput
    {
        public CreateOrEditCaseInsurerDto CaseInsurer { get; set; }

        public string CompanyName { get; set; }

    }
}