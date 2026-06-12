using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dto
{
    public class CreateMainRegistrationInput : EntityDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The CaseType field is required.")]
        public int CaseTypeId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The Branch Name field is required.")]
        public int BranchId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Insurance Company field is required.")]
        public int CompanyId { get; set; }

        [Required]
        public string VehicleNo { get; set; }

        public DateTime AssignTime { get; set; }

        public DateTime? CompletionTime { get; set; }
        [Required]
        public string ModeOfAssignment { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Adjuster field is required.")]
        public long AdjusterMemberId { get; set; }

        public long? EditorMemberId { get; set; }

        public int? StatusId { get; set; }

        public string RemarkDescription { get; set; }

    }
}
