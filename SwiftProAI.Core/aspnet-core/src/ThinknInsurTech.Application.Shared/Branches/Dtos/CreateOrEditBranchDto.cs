using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Branches.Dtos
{
    public class CreateOrEditBranchDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string ShortName { get; set; }

    }
}