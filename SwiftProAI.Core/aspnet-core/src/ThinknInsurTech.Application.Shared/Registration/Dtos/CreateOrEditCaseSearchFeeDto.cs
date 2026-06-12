using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseSearchFeeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CaseSearchFeeConsts.MaxRemarkLength, MinimumLength = CaseSearchFeeConsts.MinRemarkLength)]
        public string Remark { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Search Fee Amount must be a positive number")]
        public decimal Amount { get; set; }

        public int RegisterId { get; set; }

    }
}