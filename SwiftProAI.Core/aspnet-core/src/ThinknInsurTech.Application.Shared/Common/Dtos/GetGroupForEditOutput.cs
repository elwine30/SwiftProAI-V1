using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetGroupForEditOutput
    {
        public CreateOrEditGroupDto Group { get; set; }

        public string BranchName { get; set; }

    }
}