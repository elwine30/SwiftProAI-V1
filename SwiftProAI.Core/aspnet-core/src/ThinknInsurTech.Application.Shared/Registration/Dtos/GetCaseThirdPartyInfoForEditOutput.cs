using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseThirdPartyInfoForEditOutput
    {
        public CreateOrEditInsuredPersonDto CaseInsuredPerson { get; set; }
        public CreateOrEditCaseThirdPartyInfoDto CaseThirdPartyInfo { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string HospitalName { get; set; }

        public string HospitalAddress { get; set; }

        public string HospitalName2 { get; set; }

        public string HospitalAddress2 { get; set; }

        public string HospitalName3 { get; set; }

        public string HospitalAddress3 { get; set; }
    }
}