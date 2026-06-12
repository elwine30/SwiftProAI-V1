using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseWorkshopDto : EntityDto<int?>
    {

        public string Email { get; set; }

        [StringLength(CaseWorkshopConsts.MaxContactLength, MinimumLength = CaseWorkshopConsts.MinContactLength, ErrorMessage = "Workshop Contact Number must be between 8 and 12 digits")]
        public string ContactNo { get; set; }

        public string ContactName { get; set; }

        public int RegisterId { get; set; }

        public int? WorkshopId { get; set; }

    }
}