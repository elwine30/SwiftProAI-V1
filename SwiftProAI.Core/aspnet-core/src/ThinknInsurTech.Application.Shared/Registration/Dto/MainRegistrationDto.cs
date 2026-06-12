using Abp.Application.Services.Dto;
using System;


namespace ThinknInsurTech.Registration.Dto
{
    public class MainRegistrationDto : EntityDto
    {
        public int CaseTypeId { get; set; }

        public int BranchId { get; set; }

        public int CompanyId { get; set; }

        public string VehicleNo { get; set; }

        public DateTime AssignTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public string ModeOfAssignment { get; set; }

        public long AdjusterMemberId { get; set; }

        public long? EditorMemberId { get; set; }

        public int? StatusId { get; set; }
        public string RemarkId { get; set; }
        public string RemarkDescription { get; set; }

        public string FileRefNo { get; set; }


    }
}
