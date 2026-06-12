using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Branches.Dtos
{
    public class GetBranchForEditOutput
    {
        public CreateOrEditBranchDto Branch { get; set; }

    }
}