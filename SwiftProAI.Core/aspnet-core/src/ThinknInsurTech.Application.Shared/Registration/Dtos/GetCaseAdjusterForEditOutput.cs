using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseAdjusterForEditOutput
    {
        public CreateOrEditCaseAdjusterDto CaseAdjuster { get; set; }

        public string ScopeAssignmentDescription { get; set; }

        public int CaseTypeId { get; set; }

        public int BranchId { get; set; }

        public long AdjusterMemberId { get; set; }

        public string AdjusterContact { get; set; }

        public DateTime AssignmentTime { get; set; }

        public DateTime CompletionTime { get; set; }

    }
}