using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class CreateOrEditDocumentSettingDto : EntityDto<int?>
    {

        [StringLength(DocumentSettingConsts.MaxbusinessRegistrationNoLength, MinimumLength = DocumentSettingConsts.MinbusinessRegistrationNoLength)]
        public string businessRegistrationNo { get; set; }

        [StringLength(DocumentSettingConsts.MaxcompanyLegalNameLength, MinimumLength = DocumentSettingConsts.MincompanyLegalNameLength)]
        public string companyLegalName { get; set; }

        [StringLength(DocumentSettingConsts.MaxaddressLength, MinimumLength = DocumentSettingConsts.MinaddressLength)]
        public string address { get; set; }

        [StringLength(DocumentSettingConsts.MaxtaxNoLength, MinimumLength = DocumentSettingConsts.MintaxNoLength)]
        public string taxNo { get; set; }

        [Required]
        [StringLength(DocumentSettingConsts.MaxtelNoLength, MinimumLength = DocumentSettingConsts.MintelNoLength, ErrorMessage = "Telephone Number must be between 8 and 12 digits")]
        public string telNo { get; set; }

        [StringLength(DocumentSettingConsts.MaxinvoiceRefNoPrefixLength, MinimumLength = DocumentSettingConsts.MininvoiceRefNoPrefixLength)]
        public string invoiceRefNoPrefix { get; set; }

        [Range(DocumentSettingConsts.MininvoiceRefNoLengthValue, DocumentSettingConsts.MaxinvoiceRefNoLengthValue)]
        public int? invoiceRefNoLength { get; set; }

        [StringLength(DocumentSettingConsts.MaxdebitRefNoPrefixLength, MinimumLength = DocumentSettingConsts.MindebitRefNoPrefixLength)]
        public string debitRefNoPrefix { get; set; }

        [Range(DocumentSettingConsts.MindebitRefNoLengthValue, DocumentSettingConsts.MaxdebitRefNoLengthValue)]
        public int? debitRefNoLength { get; set; }

        [StringLength(DocumentSettingConsts.MaxcreditRefNoPrefixLength, MinimumLength = DocumentSettingConsts.MincreditRefNoPrefixLength)]
        public string creditRefNoPrefix { get; set; }

        [Range(DocumentSettingConsts.MincreditRefNoLengthValue, DocumentSettingConsts.MaxcreditRefNoLengthValue)]
        public int? creditRefNoLength { get; set; }

        [StringLength(DocumentSettingConsts.MaxcaseRefNoPrefixLength, MinimumLength = DocumentSettingConsts.MincaseRefNoPrefixLength)]
        public string caseRefNoPrefix { get; set; }

        [Range(DocumentSettingConsts.MincaseRefNoLengthValue, DocumentSettingConsts.MaxcaseRefNoLengthValue)]
        public int? caseRefNoLength { get; set; }

        public string companyType { get; set; }
        public long organizationUnitId { get; set; }
    }
}