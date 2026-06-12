using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Branches.Dtos
{
    public class GetAllBranchForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ShortNameFilter { get; set; }

    }
}