using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseInvoiceForPreviewDto
    {
        public CaseInvoiceDto CaseInvoice { get; set; }

        public List<InvoiceItemDto> InvoiceItems { get; set; }

        public string TenantCompanyName { get; set; }

        public string TenantBusinessRegistrationNo { get; set; }

        public string TenantCompanyTaxVatNo { get; set; }

        public string TenantCompanyAddress { get; set; }

        public string TenantCompanyTelNo { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanySstRegNo { get; set; }

        public string MainRegistrationVehicleNo { get; set; }

        public string InsuredPersonName { get; set; }

        public string InsuredPersonPolicyNo { get; set; }

        public string FileRefNo { get; set; }

        public int? statusId { get; set; }

    }
}
