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
    [Table("CaseDebitNotes")]
    public class CaseDebitNote : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxServiceRemarkLength, MinimumLength = CaseDebitNoteConsts.MinServiceRemarkLength)]
        public string ServiceRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Service Amount must be a positive number")]
        public decimal ServiceAmount { get; set; }

        public decimal ServiceSST { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxMileageRemarkLength, MinimumLength = CaseDebitNoteConsts.MinMileageRemarkLength)]
        public string MileageRemark { get; set; }

        public decimal MileageUnitPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mileage(KM) must be a positive number")]

        public decimal MileageKM { get; set; }

        public decimal MileageAmount { get; set; }

        public decimal MileageSST { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxPhotographRemarkLength, MinimumLength = CaseDebitNoteConsts.MinPhotographRemarkLength)]
        public string PhotographRemark { get; set; }

        public decimal PhotographCharge { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "PhotographQty must be a positive number")]
        public int PhotographQty { get; set; }

        public decimal PhotographTotalPrice { get; set; }

        public decimal PhotographSST { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxTollRemarkLength, MinimumLength = CaseDebitNoteConsts.MinTollRemarkLength)]
        public string TollRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Toll Amount must be a positive number")]
        public decimal TollAmount { get; set; }

        public decimal TollSST { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxBridgeTollRemarkLength, MinimumLength = CaseDebitNoteConsts.MinBridgeTollRemarkLength)]
        public string BridgeTollRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Bridge Toll must be a positive number")]
        public decimal BridgeTollAmount { get; set; }

        public decimal BridgeTollSST { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Police Fee must be a positive number")]
        public decimal PoliceAmount { get; set; }

        public decimal PoliceSST { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Statutory Declaration Amount must be a positive number")]
        public decimal StatutoryDeclarationAmount { get; set; }

        public decimal StatutoryDeclarationSST { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxSurveillanceRemarkLength, MinimumLength = CaseDebitNoteConsts.MinSurveillanceRemarkLength)]
        public string SurveillanceRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Surveillance Amount must be a positive number")]
        public decimal SurveillanceAmount { get; set; }

        public decimal SurveillanceSST { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "TelcoAmount must be a positive number")]
        public decimal TelcoAmount { get; set; }

        public decimal TelcoSST { get; set; }

        public decimal ThirdPartyAmount { get; set; }

        public decimal ThirdPartySST { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxCourtAttendanceRemarkLength, MinimumLength = CaseDebitNoteConsts.MinCourtAttendanceRemarkLength)]
        public string CourtAttendanceRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Court Attendance must be a positive number")]
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

        [StringLength(CaseDebitNoteConsts.MaxInvoiceFlagLength, MinimumLength = CaseDebitNoteConsts.MinInvoiceFlagLength)]
        public string InvoiceFlag { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxInvoiceTypeLength, MinimumLength = CaseDebitNoteConsts.MinInvoiceTypeLength)]
        public string InvoiceType { get; set; }

        public string DebitRefNo { get; set; }

        public string DebitRefNoPrefix { get; set; }

        public DateTime? DebitDate { get; set; }

        public decimal? NetAmount { get; set; }

        public string CheckNo { get; set; }

        public DateTime? DatePaid { get; set; }

        public string Lawyer { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxPaymentFlagLength, MinimumLength = CaseDebitNoteConsts.MinPaymentFlagLength)]
        public string PaymentFlag { get; set; }

        public decimal? AmountPaid { get; set; }

        [StringLength(CaseDebitNoteConsts.MaxPaymentModeLength, MinimumLength = CaseDebitNoteConsts.MinPaymentModeLength)]
        public string PaymentMode { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public virtual int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public InsuranceCompany CompanyFk { get; set; }

        public virtual long? ClaimExecutive { get; set; }

        [ForeignKey("ClaimExecutive")]
        public User ClaimExecutiveFk { get; set; }

        public virtual long AdjusterId { get; set; }

        [ForeignKey("AdjusterId")]
        public User AdjusterFk { get; set; }

        public virtual int CaseTypeId { get; set; }

        [ForeignKey("CaseTypeId")]
        public CaseType CaseTypeFk { get; set; }
        public long? OrganizationUnitId { get; set; }


    }
}