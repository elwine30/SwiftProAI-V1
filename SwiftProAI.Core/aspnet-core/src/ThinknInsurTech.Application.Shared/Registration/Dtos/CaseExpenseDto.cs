using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseExpenseDto : EntityDto
    {
        public double Amount { get; set; }

        public double ApprovedAmount { get; set; }

        public string Remark { get; set; }

        public int StatusId { get; set; }

        public int? TypeId { get; set; }

        public int? SubTypeId { get; set; }

        public int RegisterId { get; set; }

        public long AdjusterId { get; set; }

    }
}