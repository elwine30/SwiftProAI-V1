using ThinknInsurTech.Companies.Dtos;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseInvoiceForEditOutput
    {
        public CreateOrEditCaseInvoiceDto CaseInvoice { get; set; }

        public CaseClaimDto CaseClaim { get; set; }

        public CompanyDto Company { get; set; }

        public string CompanyName { get; set; }

        public string TenantCompanyName { get; set; }

        public string ClaimExecutiveUserName { get; set; }

        public string AdjusterUserName { get; set; }

        public string CaseTypeShortName { get; set; }

        public decimal MileageUnitPrice { get; set; }

        public decimal SSTRate { get; set; }

        public decimal PhotographCharge { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public int? CaseStatusId { get; set; }

    }
}