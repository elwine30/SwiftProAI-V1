using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class StaffDto : EntityDto
    {
        public string NRIC { get; set; }

        public string Address { get; set; }

        public string Passport { get; set; }

        public decimal? ServiceFeePercent { get; set; }

        public decimal? FraudFeePercent { get; set; }

        public long UserId { get; set; }

        public int? GroupId { get; set; }

    }
}