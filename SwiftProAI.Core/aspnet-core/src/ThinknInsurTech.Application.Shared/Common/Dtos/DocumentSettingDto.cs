using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class DocumentSettingDto : EntityDto
    {
        public string businessRegistrationNo { get; set; }

        public string companyLegalName { get; set; }

        public string address { get; set; }

        public string taxNo { get; set; }

        public string telNo { get; set; }

        public string invoiceRefNoPrefix { get; set; }

        public int? invoiceRefNoLength { get; set; }

        public string debitRefNoPrefix { get; set; }

        public int? debitRefNoLength { get; set; }

        public string creditRefNoPrefix { get; set; }

        public int? creditRefNoLength { get; set; }

        public string caseRefNoPrefix { get; set; }

        public int? caseRefNoLength { get; set; }

        public string companyType { get; set; }
    }
}