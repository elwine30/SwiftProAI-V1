namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseInvoiceForViewDto
    {
        public CaseInvoiceDto CaseInvoice { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string CompanyName { get; set; }

        public string ClaimExecutiveUserName { get; set; }

        public string AdjusterUserName { get; set; }

        public string CaseTypeShortName { get; set; }

    }
}