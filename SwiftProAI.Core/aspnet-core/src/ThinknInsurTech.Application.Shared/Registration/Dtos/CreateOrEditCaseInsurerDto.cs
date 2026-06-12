using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseInsurerDto : EntityDto<int?>
    {

        public string ReferenceNo { get; set; }

        public string Name { get; set; }

        [StringLength(CaseInsurerConsts.MaxContactLength, MinimumLength = CaseInsurerConsts.MinContactLength, ErrorMessage = "Insurer Contact Number must be between 8 and 12 digits")]
        public string Contact { get; set; }

        public string Email { get; set; }

        public int RegisterId { get; set; }

        public int? CompanyId { get; set; }

    }
}