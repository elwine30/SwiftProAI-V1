using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration
{
    public class UpdateCaseExpensesDTO : Entity<int?>
    {
        public double Amount { get; set; }

        public double ApprovedAmount { get; set; }

        public string Remark { get; set; }

        public bool Aprroved { get; set; }

        public bool Rejected { get; set; }

        public int StatusId { get; set; }

        public int? TypeId { get; set; }

        public int? SubTypeId { get; set; }

        public int RegisterId { get; set; }

        public long AdjusterId { get; set; }
    }
}
