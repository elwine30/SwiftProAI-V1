using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseInsurersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ReferenceNoFilter { get; set; }

        public string NameFilter { get; set; }

        public string ContactFilter { get; set; }

        public string EmailFilter { get; set; }

        public int? MaxRegisterIdFilter { get; set; }
        public int? MinRegisterIdFilter { get; set; }

        public string CompanyNameFilter { get; set; }

    }
}