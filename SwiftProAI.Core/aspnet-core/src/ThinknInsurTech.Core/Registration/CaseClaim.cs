using ThinknInsurTech.Registration;
using ThinknInsurTech.Case;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using ThinknInsurTech.Common;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseClaims")]
    [Auditable]
    public class CaseClaim : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }
        
        public int? CaseNo { get; set; }
        [AuditedTrail]
        public decimal Total { get; set; }
        [AuditedTrail]
        public string Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "File Charges must be a positive number")]
        [AuditedTrail]
        public decimal FileCharges { get; set; }

        [StringLength(CaseClaimConsts.MaxFileChargesRemarkLength, MinimumLength = CaseClaimConsts.MinFileChargesRemarkLength)]
        [AuditedTrail]
        public string FileChargesRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SD must be a positive number")]
        [AuditedTrail]
        public decimal SD { get; set; }

        [AuditedTrail]
        public decimal SearchFee { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hotel Fees must be a positive number")]
        [AuditedTrail]
        public decimal Hotel { get; set; }

        [AuditedTrail]
        public bool Fraud { get; set; }

        [AuditedTrail]
        public decimal FraudAmount { get; set; }

        [AuditedTrail]
        public string Remarks { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Police Fees must be a positive number")]
        [AuditedTrail]
        public decimal Police { get; set; }

        [AuditedTrail]
        public string PoliceRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Air Fare must be a positive number")]
        [AuditedTrail]
        public decimal AirFare { get; set; }

        [AuditedTrail]
        public string AirFareRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Chartered Transport Fees must be a positive number")]
        [AuditedTrail]
        public decimal CharteredTransport { get; set; }

        [AuditedTrail]
        public string CharteredTransportRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Toll Fees must be a positive number")]
        [AuditedTrail]
        public decimal Toll { get; set; }

        [AuditedTrail]
        public string TollRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mileage must be a positive number")]
        [AuditedTrail]
        public decimal MileageKM { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mileage Unit Price must be a positive number")]
        [AuditedTrail]
        public decimal MileageUnitPrice { get; set; }

        [AuditedTrail]
        public decimal MileageTotal { get; set; }

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public virtual int? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Lookup StatusFk { get; set; }

        [AuditedTrail]
        public bool Approved { get; set; }

        [AuditedTrail]
        public bool Rejected { get; set; }
        public long? OrganizationUnitId { get; set; }


    }
}