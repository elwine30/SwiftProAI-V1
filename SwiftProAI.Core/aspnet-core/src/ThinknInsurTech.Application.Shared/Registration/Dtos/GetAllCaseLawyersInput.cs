using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseLawyersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxHearingDateFilter { get; set; }
        public DateTime? MinHearingDateFilter { get; set; }

        public string ReferenceNoFilter { get; set; }

        public string ContactNoFilter { get; set; }

        public string ContactNameFilter { get; set; }

        public string EmailFilter { get; set; }

        public string TypeFilter { get; set; }

        public string RegisterIdFilter { get; set; }

        public string LawFirmNameFilter { get; set; }

    }
}