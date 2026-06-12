using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllDocumentSettingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BusinessRegistrationNoFilter { get; set; }

        public string companyLegalNameFilter { get; set; }

        public string addressFilter { get; set; }

        public string taxNoFilter { get; set; }

        public string telNoFilter { get; set; }

        public string invoiceRefNoPrefixFilter { get; set; }

        public int? MaxinvoiceRefNoLengthFilter { get; set; }
        public int? MininvoiceRefNoLengthFilter { get; set; }

        public string debitRefNoPrefixFilter { get; set; }

        public int? MaxdebitRefNoLengthFilter { get; set; }
        public int? MindebitRefNoLengthFilter { get; set; }

        public string creditRefNoPrefixFilter { get; set; }

        public int? MaxcreditRefNoLengthFilter { get; set; }
        public int? MincreditRefNoLengthFilter { get; set; }

        public string caseRefNoPrefixFilter { get; set; }

        public int? MaxcaseRefNoLengthFilter { get; set; }
        public int? MincaseRefNoLengthFilter { get; set; }

    }
}