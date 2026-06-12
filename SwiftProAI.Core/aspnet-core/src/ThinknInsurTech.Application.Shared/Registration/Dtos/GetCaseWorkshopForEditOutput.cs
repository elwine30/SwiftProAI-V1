using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseWorkshopForEditOutput
    {
        public CreateOrEditCaseWorkshopDto CaseWorkshop { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string WorkshopWorkshopName { get; set; }

    }
}