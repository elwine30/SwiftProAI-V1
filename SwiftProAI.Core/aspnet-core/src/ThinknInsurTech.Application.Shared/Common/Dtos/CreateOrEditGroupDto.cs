using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class CreateOrEditGroupDto : EntityDto<int?>
    {

        [Required]
        [StringLength(GroupConsts.MaxNameLength, MinimumLength = GroupConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [Range(GroupConsts.MinGroupTypeValue, GroupConsts.MaxGroupTypeValue)]
        public int GroupType { get; set; }

        public bool IsActive { get; set; }

        public int BranchId { get; set; }

    }
}