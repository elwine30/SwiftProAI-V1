namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseThirdPartyInfoForViewDto
    {
        public CaseThirdPartyInfoDto CaseThirdPartyInfo { get; set; }

        public InsuredPersonDto InsuredPerson { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }


        public string HospitalName2 { get; set; }
        public string HospitalAddress2 { get; set; }


        public string HospitalName3 { get; set; }
        public string HospitalAddress3 { get; set; }


        public string CaseInsuredPersonName { get; set; }

        public string caseInsuredCountry { get; set; }
        public string caseInsuredState { get; set; }

        public string EmpToken { get; set; }
        public string EmpFileName { get; set; }

        public string DetToken { get; set; }
        public string DetFileName { get; set; }

        public string DlDetBackToken { get; set; }
        public string DlDetBackFileName { get; set; }

        public string DlDetFrontToken { get; set; }
        public string DlDetFrontFileName { get; set; }

        public string DlNricBackToken { get; set; }
        public string DlNricBackFileName { get; set; }

        public string NricDetFrontToken { get; set; }
        public string NricDetFrontFileName { get; set; }

        public string NoiToken { get; set; }
        public string NoiFileName { get; set; }

        public string HospToken { get; set; }
        public string HospFileName { get; set; }


    }
}