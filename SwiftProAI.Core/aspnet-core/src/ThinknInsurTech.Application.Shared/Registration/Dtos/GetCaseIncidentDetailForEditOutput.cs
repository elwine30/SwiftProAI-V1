using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseIncidentDetailForEditOutput
    {
        public CreateOrEditCaseIncidentDetailDto CaseIncidentDetail { get; set; }

        public string CircumstancesFileUploadFileName { get; set; }

    }
}