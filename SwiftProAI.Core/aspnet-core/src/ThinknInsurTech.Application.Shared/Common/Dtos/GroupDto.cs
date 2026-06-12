using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class GroupDto : EntityDto
    {
        public string Name { get; set; }

        public int GroupType { get; set; }

        public bool IsActive { get; set; }

        public int BranchId { get; set; }

    }
}