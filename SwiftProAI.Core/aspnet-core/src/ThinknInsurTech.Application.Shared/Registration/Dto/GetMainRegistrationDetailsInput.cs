using System;
using Abp.Runtime.Validation;
using ThinknInsurTech.Common;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Registration.Dto
{
    public class GetMainRegistrationDetailsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public DateTime? AssignmentDateStart { get; set; }
        public DateTime? AssignmentDateEnd { get; set; }
        public int? StatusId { get; set; }
        public bool StatusIdSpecified { get; set; }
        public int? CompanyId { get; set; }
        public bool CompanyIdSpecified { get; set; }
        public int? AdjusterMemberId { get; set; }
        public bool AdjusterIdSpecified { get; set; }
        public int? EditorMemberId { get; set; }
        public bool EditorIdSpecified { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }

            Sorting = DtoSortingHelper.ReplaceSorting(Sorting, s =>
            {
                if (s.Contains("CaseTypeShortName"))
                    return s.Replace("CaseTypeShortName", "CaseType.ShortName");
                else if (s.Contains("BranchShortName"))
                    return s.Replace("BranchShortName", "Branch.ShortName");
                else if (s.Contains("CompanyShortName"))
                    return s.Replace("CompanyShortName", "Company.ShortName");
                else if (s.Contains("StatusCode"))
                    return s.Replace("StatusCode", "Status.Code");
                else
                    return s;
            });
        }
    }
}
