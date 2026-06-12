using ThinknInsurTech.Registration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CasePoliceReports")]
    [Auditable]
    public class CasePoliceReport : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [StringLength(CasePoliceReportConsts.MaxIPDLength, MinimumLength = CasePoliceReportConsts.MinIPDLength)]
        [AuditedTrail]
        public string IPD { get; set; }

        [StringLength(CasePoliceReportConsts.MaxPoliceStationLength, MinimumLength = CasePoliceReportConsts.MinPoliceStationLength)]
        [AuditedTrail]
        public string PoliceStation { get; set; }

        [StringLength(CasePoliceReportConsts.MaxVehicleNoLength, MinimumLength = CasePoliceReportConsts.MinVehicleNoLength)]
        [AuditedTrail]
        public string VehicleNo { get; set; }

        [StringLength(CasePoliceReportConsts.MaxReportNoLength, MinimumLength = CasePoliceReportConsts.MinReportNoLength)]
        [AuditedTrail]
        public string ReportNo { get; set; }
        
        [AuditedTrail]
        public DateTime ReportTime { get; set; }
        [AuditedTrail]
        public DateTime IncidentTime { get; set; }

        [StringLength(CasePoliceReportConsts.MaxLateReportLength, MinimumLength = CasePoliceReportConsts.MinLateReportLength)]
        [AuditedTrail]
        public string LateReport { get; set; }

        [AuditedTrail]
        public string LateReason { get; set; }

        [AuditedTrail]
        public string OfficerName { get; set; }
        [AuditedTrail]
        public string ServiceNo { get; set; }
        [AuditedTrail]
        public string OfficerContact { get; set; }

        [StringLength(CasePoliceReportConsts.MaxTypeLength, MinimumLength = CasePoliceReportConsts.MinTypeLength)]
        [AuditedTrail]
        public string Type { get; set; }

        [AuditedTrail]
        public string PoliceFinding { get; set; }

        [AuditedTrail]
        public string PoliceOutcome { get; set; }
        //File

        [AuditedTrail]
        public virtual Guid? ReportFileUpload { get; set; } //File, (BinaryObjectId)

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public long? OrganizationUnitId { get; set; }

        public string ReportType { get; set; }

        public bool IsDataConsistent { get; set; }

        [StringLength(CasePoliceReportConsts.MaxStatementLength, MinimumLength = CasePoliceReportConsts.MinStatementLength)]
        public string Statement { get; set; }

        public string ComplainantIdentityNo { get; set; }
    }
}