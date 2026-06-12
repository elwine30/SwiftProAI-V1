using ThinknInsurTech.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Reports
{
    //[Table("AdjusterReports")]
    public class AdjusterReport 
    {
        public int? TenantId { get; set; }

        public int Month { get; set; }

        public  int Year { get; set; }

        public  long? AdjusterId { get; set; }

        [ForeignKey("AdjusterId")]
        public User AdjusterFk { get; set; }


    }
}