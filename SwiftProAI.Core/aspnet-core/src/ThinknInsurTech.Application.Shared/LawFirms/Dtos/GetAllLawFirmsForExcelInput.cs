using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.LawFirms.Dtos
{
    public class GetAllLawFirmsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ShortNameFilter { get; set; }

        public string AddressFilter { get; set; }

    }
}