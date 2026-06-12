namespace ThinknInsurTech.Registration.Dtos
{
    public class GetInsuredPersonForViewDto
    {
        public InsuredPersonDto InsuredPerson { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }

        public string LocationName { get; set; }

        public string LocationName2 { get; set; }

    }
}