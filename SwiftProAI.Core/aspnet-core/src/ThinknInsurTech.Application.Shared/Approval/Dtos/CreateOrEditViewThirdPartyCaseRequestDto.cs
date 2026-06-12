using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Approval.Dtos
{
    public class CreateOrEditViewThirdPartyCaseRequestDto : EntityDto<int?>
    {

        public string Status { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? RejectedDate { get; set; }

        public int? RejectedBy { get; set; }

        public long? AssignByOU { get; set; }

        public long? AssignToOU { get; set; }

        public DateTime? CancelledDate { get; set; }

        public int? CancelledBy { get; set; }

        public string CancelRemark { get; set; }

        public string BusinessRegistrationNo { get; set; }

        public string CompanyName { get; set; }
    }
}