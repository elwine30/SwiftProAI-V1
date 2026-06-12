using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetStaffForEditOutput
    {
        public CreateOrEditStaffDto Staff { get; set; }

        public string UserName { get; set; }

        public string GroupName { get; set; }

    }
}