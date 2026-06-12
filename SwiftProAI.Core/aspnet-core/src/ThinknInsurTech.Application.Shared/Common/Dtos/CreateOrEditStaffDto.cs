using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class CreateOrEditStaffDto : EntityDto<int?>
    {

        [StringLength(StaffConsts.MaxNRICLength, MinimumLength = StaffConsts.MinNRICLength)]
        public string NRIC { get; set; }

        [StringLength(StaffConsts.MaxAddressLength, MinimumLength = StaffConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(StaffConsts.MaxPassportLength, MinimumLength = StaffConsts.MinPassportLength)]
        public string Passport { get; set; }

        [Range(StaffConsts.MinServiceFeePercentValue, StaffConsts.MaxServiceFeePercentValue)]
        public decimal? ServiceFeePercent { get; set; }

        [Range(StaffConsts.MinFraudFeePercentValue, StaffConsts.MaxFraudFeePercentValue)]
        public decimal? FraudFeePercent { get; set; }

        public long UserId { get; set; }

        public int? GroupId { get; set; }

    }
}