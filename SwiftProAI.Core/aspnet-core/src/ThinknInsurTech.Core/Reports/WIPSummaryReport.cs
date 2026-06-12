using ThinknInsurTech.Case;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Companies;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ThinknInsurTech.Reports
{
    //[Table("WIPSummaryReports")]
    public class WIPSummaryReport 
    {
        public int? TenantId { get; set; }

        public int? CaseTypeId { get; set; }

        public long? UserId { get; set; }

        public int? CompanyId { get; set; }

    }
}