using ThinknInsurTech.Registration;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Case;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseCreditNotes")]
    public class CaseCreditNote : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxServiceRemarkLength, MinimumLength = CaseCreditNoteConsts.MinServiceRemarkLength)]
        public string ServiceRemark { get; set; }

        public decimal ServiceAmount { get; set; }

        public decimal ServiceSST { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxMileageRemarkLength, MinimumLength = CaseCreditNoteConsts.MinMileageRemarkLength)]
        public string MileageRemark { get; set; }

        public decimal MileageUnitPrice { get; set; }

        public decimal MileageKM { get; set; }

        public decimal MileageAmount { get; set; }

        public decimal MileageSST { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxPhotographRemarkLength, MinimumLength = CaseCreditNoteConsts.MinPhotographRemarkLength)]
        public string PhotographRemark { get; set; }

        public decimal PhotographCharge { get; set; }

        public int PhotographQty { get; set; }

        public decimal PhotographTotalPrice { get; set; }

        public decimal PhotographSST { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxTollRemarkLength, MinimumLength = CaseCreditNoteConsts.MinTollRemarkLength)]
        public string TollRemark { get; set; }

        public decimal TollAmount { get; set; }

        public decimal TollSST { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxBridgeTollRemarkLength, MinimumLength = CaseCreditNoteConsts.MinBridgeTollRemarkLength)]
        public string BridgeTollRemark { get; set; }

        public decimal BridgeTollAmount { get; set; }

        public decimal BridgeTollSST { get; set; }

        public decimal PoliceAmount { get; set; }

        public decimal PoliceSST { get; set; }

        public decimal StatutoryDeclarationAmount { get; set; }

        public decimal StatutoryDeclarationSST { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxSurveillanceRemarkLength, MinimumLength = CaseCreditNoteConsts.MinSurveillanceRemarkLength)]
        public string SurveillanceRemark { get; set; }

        public decimal SurveillanceAmount { get; set; }

        public decimal SurveillanceSST { get; set; }

        public decimal TelcoAmount { get; set; }

        public decimal TelcoSST { get; set; }

        public decimal ThirdPartyAmount { get; set; }

        public decimal ThirdPartySST { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxCourtAttendanceRemarkLength, MinimumLength = CaseCreditNoteConsts.MinCourtAttendanceRemarkLength)]
        public string CourtAttendanceRemark { get; set; }

        public decimal CourtAttendanceAmount { get; set; }

        public decimal CourtAttendanceSST { get; set; }

        public decimal SearchFeeAmount { get; set; }

        public decimal SearchFeeSST { get; set; }

        public decimal AirFareAmount { get; set; }

        public decimal AirFareSST { get; set; }

        public decimal CharteredAmount { get; set; }

        public decimal CharteredSST { get; set; }

        public decimal TaxiFareAmount { get; set; }

        public decimal TaxiFareSST { get; set; }

        public decimal AccommodationAmount { get; set; }

        public decimal AccommodationSST { get; set; }

        public decimal MiscAmount { get; set; }

        public decimal MiscSST { get; set; }

        public decimal AmountExcludeSST { get; set; }

        public decimal AmountWithSST { get; set; }

        public bool IncludeSST { get; set; }

        public string TotalInTextForm { get; set; }

        public decimal TotalAmount { get; set; }

        public string InvoiceRefNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxInvoiceFlagLength, MinimumLength = CaseCreditNoteConsts.MinInvoiceFlagLength)]
        public string InvoiceFlag { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxInvoiceTypeLength, MinimumLength = CaseCreditNoteConsts.MinInvoiceTypeLength)]
        public string InvoiceType { get; set; }

        public string CreditRefNo { get; set; }

        public string CreditRefNoPrefix { get; set; }

        public DateTime? DebitDate { get; set; }

        public decimal? NetAmount { get; set; }

        public string CheckNo { get; set; }

        public DateTime? DatePaid { get; set; }

        public string Lawyer { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxPaymentFlagLength, MinimumLength = CaseCreditNoteConsts.MinPaymentFlagLength)]
        public string PaymentFlag { get; set; }

        public decimal? AmountPaid { get; set; }

        public decimal? CreditAmount { get; set; }

        public decimal? DebitAmount { get; set; }

        [StringLength(CaseCreditNoteConsts.MaxPaymentModeLength, MinimumLength = CaseCreditNoteConsts.MinPaymentModeLength)]
        public string PaymentMode { get; set; }

        public int? RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public InsuranceCompany CompanyFk { get; set; }

        public long? ClaimExecutive { get; set; }

        [ForeignKey("ClaimExecutive")]
        public User ClaimExecutiveFk { get; set; }

        public long? AdjusterId { get; set; }

        [ForeignKey("AdjusterId")]
        public User AdjusterFk { get; set; }

        public int? CaseTypeId { get; set; }

        [ForeignKey("CaseTypeId")]
        public CaseType CaseTypeFk { get; set; }
        public long? OrganizationUnitId { get; set; }


    }
}