using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dto
{
    public class MainRegistrationDashboardDto : EntityDto
    {
        public int CaseTypeId { get; set; }

        public string CaseTypeShortName { get; set; }

        public int BranchId { get; set; }

        public string BranchShortName { get; set; }

        public int CompanyId { get; set; }

        public string CompanyShortName { get; set; }

        public string VehicleNo { get; set; }

        public DateTime AssignTime { get; set; }

        public string ModeOfAssignment { get; set; }

        public string AdjusterUserName { get; set; }

        public long? EditorMemberId { get; set; }

        public int? StatusId { get; set; }

        public string StatusCode { get; set; }

        public string Prefix { get; set; }

        public string CaseNo { get; set; }

        public bool? IsReadOnly { get; set; }
    }
}
