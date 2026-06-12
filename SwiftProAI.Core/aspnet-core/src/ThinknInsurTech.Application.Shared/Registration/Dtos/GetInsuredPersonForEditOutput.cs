using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetInsuredPersonForEditOutput
    {
        public CreateOrEditInsuredPersonDto InsuredPerson { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }

        public string LocationName { get; set; }

        public string LocationName2 { get; set; }

        public string DriverICFrontFileName { get; set; }

        public string DriverICBackFileName { get; set; }

        public string DriverLicenseFrontFileName { get; set; }

        public string DriverLicenseBackFileName { get; set; }

        public string DriverEmploymentDetailFileName { get; set; }

        public string DriverHospitalDetailFileName { get; set; }

        public string DriverCarGrantFileName { get; set; }

    }
}