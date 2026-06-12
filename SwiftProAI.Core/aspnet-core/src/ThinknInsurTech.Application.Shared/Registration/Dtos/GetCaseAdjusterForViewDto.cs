using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseAdjusterForViewDto
    {
        public CaseAdjusterDto CaseAdjuster { get; set; }

        public string ScopeAssignmentDescription { get; set; }

        public string RegistrationCaseTypeId { get; set; }

        public string AdjusterName { get; set; }

        public string AdjusterContact { get; set; }

        public DateTime? AssignmentTime { get; set; }

        public DateTime? CompletionTime { get; set; }
        public string EditorUserName { get; set; }
        public string CaseTypeName { get; set; }
        public string BranchName { get; set; }
        public string StateName { get; set; }


    }
}