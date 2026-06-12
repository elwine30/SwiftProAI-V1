using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Organizations;

namespace ThinknInsurTech.Common
{
    [Table("DocumentSettings")]
    public class DocumentSetting : FullAuditedEntity, IMustHaveTenant, IMustHaveOrganizationUnit
    {
        public long OrganizationUnitId { get; set; }

        public int TenantId { get; set; }

        [StringLength(DocumentSettingConsts.MaxbusinessRegistrationNoLength, MinimumLength = DocumentSettingConsts.MinbusinessRegistrationNoLength)]
        public string businessRegistrationNo { get; set; }

        [StringLength(DocumentSettingConsts.MaxcompanyLegalNameLength, MinimumLength = DocumentSettingConsts.MincompanyLegalNameLength)]
        public string companyLegalName { get; set; }

        [StringLength(DocumentSettingConsts.MaxaddressLength, MinimumLength = DocumentSettingConsts.MinaddressLength)]
        public string address { get; set; }

        [StringLength(DocumentSettingConsts.MaxtaxNoLength, MinimumLength = DocumentSettingConsts.MintaxNoLength)]
        public string taxNo { get; set; }

        [StringLength(DocumentSettingConsts.MaxtelNoLength, MinimumLength = DocumentSettingConsts.MintelNoLength)]
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
    }
}