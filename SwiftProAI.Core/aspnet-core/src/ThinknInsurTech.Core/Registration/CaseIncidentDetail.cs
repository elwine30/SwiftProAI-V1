using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseIncidentDetails")]
    [Auditable]
    public class CaseIncidentDetail : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [AuditedTrail]
        public DateTime TimeFrom { get; set; }

        [AuditedTrail]
        public DateTime TimeTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxCountryLength, MinimumLength = CaseIncidentDetailConsts.MinCountryLength)]
        [AuditedTrail]
        public string Country { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxStateLength, MinimumLength = CaseIncidentDetailConsts.MinStateLength)]
        [AuditedTrail]
        public string State { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxPostcodeLength, MinimumLength = CaseIncidentDetailConsts.MinPostcodeLength)]
        [AuditedTrail]
        public string Postcode { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxCityLength, MinimumLength = CaseIncidentDetailConsts.MinCityLength)]
        [AuditedTrail]
        public string City { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxAddress1Length, MinimumLength = CaseIncidentDetailConsts.MinAddress1Length)]
        [AuditedTrail]
        public string Address1 { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxAddress2Length, MinimumLength = CaseIncidentDetailConsts.MinAddress2Length)]
        [AuditedTrail]
        public string Address2 { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDirectionFromLength, MinimumLength = CaseIncidentDetailConsts.MinDirectionFromLength)]
        [AuditedTrail]
        public string DirectionFrom { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDirectionToLength, MinimumLength = CaseIncidentDetailConsts.MinDirectionToLength)]
        [AuditedTrail]
        public string DirectionTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDirectionViaLength, MinimumLength = CaseIncidentDetailConsts.MinDirectionViaLength)]
        [AuditedTrail]
        public string DirectionVia { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDriverDrivingWithLength, MinimumLength = CaseIncidentDetailConsts.MinDriverDrivingWithLength)]
        [AuditedTrail]
        public string DriverDrivingWith { get; set; }

        [Range(CaseIncidentDetailConsts.MinPassengerNoValue, CaseIncidentDetailConsts.MaxPassengerNoValue)]
        [AuditedTrail]
        public int? PassengerNo { get; set; }

        [AuditedTrail]
        public string PassengerRemarks { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxSiteOfAccidentLength, MinimumLength = CaseIncidentDetailConsts.MinSiteOfAccidentLength)]
        [AuditedTrail]
        public string SiteOfAccident { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxTypeOfRoadLength, MinimumLength = CaseIncidentDetailConsts.MinTypeOfRoadLength)]
        [AuditedTrail]
        public string TypeOfRoad { get; set; }

        [AuditedTrail]
        public double? WidthOfRoad { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxCenterDemarcationLength, MinimumLength = CaseIncidentDetailConsts.MinCenterDemarcationLength)]
        [AuditedTrail]
        public string CenterDemarcation { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDriverPathLeftLength, MinimumLength = CaseIncidentDetailConsts.MinDriverPathLeftLength)]
        [AuditedTrail]
        public string DriverPathLeft { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDriverPathRightLength, MinimumLength = CaseIncidentDetailConsts.MinDriverPathRightLength)]
        [AuditedTrail]
        public string DriverPathRight { get; set; }

        [AuditedTrail]
        public string Circumstances { get; set; }

        [Range(CaseIncidentDetailConsts.MinSpeedPriorValue, CaseIncidentDetailConsts.MaxSpeedPriorValue)]
        [AuditedTrail]
        public double? SpeedPrior { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxViewOfRoadLength, MinimumLength = CaseIncidentDetailConsts.MinViewOfRoadLength)]
        [AuditedTrail]
        public string ViewOfRoad { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxViewDueToLength, MinimumLength = CaseIncidentDetailConsts.MinViewDueToLength)]
        [AuditedTrail]
        public string ViewDueTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxVisibilityLength, MinimumLength = CaseIncidentDetailConsts.MinVisibilityLength)]
        [AuditedTrail]
        public string Visibility { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxVisibilityDueToLength, MinimumLength = CaseIncidentDetailConsts.MinVisibilityDueToLength)]
        [AuditedTrail]
        public string VisibilityDueTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxSurroundingAreaLength, MinimumLength = CaseIncidentDetailConsts.MinSurroundingAreaLength)]
        [AuditedTrail]
        public string SurroundingArea { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxSurroundingLocalityLength, MinimumLength = CaseIncidentDetailConsts.MinSurroundingLocalityLength)]
        [AuditedTrail]
        public string SurroundingLocality { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxRoadConditionLength, MinimumLength = CaseIncidentDetailConsts.MinRoadConditionLength)]
        [AuditedTrail]
        public string RoadCondition { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxWeatherConditionLength, MinimumLength = CaseIncidentDetailConsts.MinWeatherConditionLength)]
        [AuditedTrail]
        public string WeatherCondition { get; set; }
        //File

        public virtual Guid? CircumstancesFileUpload { get; set; } //File, (BinaryObjectId)

        public virtual int RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}