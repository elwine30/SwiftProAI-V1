using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseLawyerDto : EntityDto<int?>
    {

        public DateTime? HearingDate { get; set; }

        public string ReferenceNo { get; set; }

        [StringLength(CaseLawyerConsts.MaxContactLength, MinimumLength = CaseLawyerConsts.MinContactLength, ErrorMessage = "Lawyer Contact Number must be between 8 and 12 digits")]
        public string ContactNo { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Type { get; set; }

        public int RegisterId { get; set; }

        public int LawFirmId { get; set; }

    }
}