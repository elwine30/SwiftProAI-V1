using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseThirdPartyVehicleForEditOutput
    {
        public CreateOrEditCaseThirdPartyVehicleDto CaseThirdPartyVehicle { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string CompanyName { get; set; }

        public string DriverCarGrantFileName { get; set; }

    }
}