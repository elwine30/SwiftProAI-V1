using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseAdjusterDto : EntityDto<int?>
    {

        public string Status { get; set; }

        public int? ScopeAssignmentId { get; set; }

        public int RegisterId { get; set; }

        public int? StateLocationId { get; set; }

        public long? EditorUserId { get; set; }

        public string ScopeAssignmentRemarks { get; set; }

        public DateTime? ExtendedCompletionDate { get; set; }

        public string ExtendCompletionRemark { get; set; }
    }
}