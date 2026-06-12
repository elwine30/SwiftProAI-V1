using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseIncidentDetailDto : EntityDto<int?>
    {
        [Required]
        public DateTime TimeFrom { get; set; }

        [Required]
        public DateTime TimeTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxCountryLength, MinimumLength = CaseIncidentDetailConsts.MinCountryLength)]
        public string Country { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxStateLength, MinimumLength = CaseIncidentDetailConsts.MinStateLength)]
        public string State { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxPostcodeLength, MinimumLength = CaseIncidentDetailConsts.MinPostcodeLength)]
        public string Postcode { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxCityLength, MinimumLength = CaseIncidentDetailConsts.MinCityLength)]
        public string City { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxAddress1Length, MinimumLength = CaseIncidentDetailConsts.MinAddress1Length)]
        public string Address1 { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxAddress2Length, MinimumLength = CaseIncidentDetailConsts.MinAddress2Length)]
        public string Address2 { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDirectionFromLength, MinimumLength = CaseIncidentDetailConsts.MinDirectionFromLength)]
        public string DirectionFrom { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDirectionToLength, MinimumLength = CaseIncidentDetailConsts.MinDirectionToLength)]
        public string DirectionTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDirectionViaLength, MinimumLength = CaseIncidentDetailConsts.MinDirectionViaLength)]
        public string DirectionVia { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDriverDrivingWithLength, MinimumLength = CaseIncidentDetailConsts.MinDriverDrivingWithLength)]
        public string DriverDrivingWith { get; set; }

        [Range(CaseIncidentDetailConsts.MinPassengerNoValue, CaseIncidentDetailConsts.MaxPassengerNoValue)]
        public int? PassengerNo { get; set; }

        public string PassengerRemarks { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxSiteOfAccidentLength, MinimumLength = CaseIncidentDetailConsts.MinSiteOfAccidentLength)]
        public string SiteOfAccident { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxTypeOfRoadLength, MinimumLength = CaseIncidentDetailConsts.MinTypeOfRoadLength)]
        public string TypeOfRoad { get; set; }

        public double? WidthOfRoad { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxCenterDemarcationLength, MinimumLength = CaseIncidentDetailConsts.MinCenterDemarcationLength)]
        public string CenterDemarcation { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDriverPathLeftLength, MinimumLength = CaseIncidentDetailConsts.MinDriverPathLeftLength)]
        public string DriverPathLeft { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxDriverPathRightLength, MinimumLength = CaseIncidentDetailConsts.MinDriverPathRightLength)]
        public string DriverPathRight { get; set; }

        public string Circumstances { get; set; }

        [Range(CaseIncidentDetailConsts.MinSpeedPriorValue, CaseIncidentDetailConsts.MaxSpeedPriorValue)]
        public double? SpeedPrior { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxViewOfRoadLength, MinimumLength = CaseIncidentDetailConsts.MinViewOfRoadLength)]
        public string ViewOfRoad { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxViewDueToLength, MinimumLength = CaseIncidentDetailConsts.MinViewDueToLength)]
        public string ViewDueTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxVisibilityLength, MinimumLength = CaseIncidentDetailConsts.MinVisibilityLength)]
        public string Visibility { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxVisibilityDueToLength, MinimumLength = CaseIncidentDetailConsts.MinVisibilityDueToLength)]
        public string VisibilityDueTo { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxSurroundingAreaLength, MinimumLength = CaseIncidentDetailConsts.MinSurroundingAreaLength)]
        public string SurroundingArea { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxSurroundingLocalityLength, MinimumLength = CaseIncidentDetailConsts.MinSurroundingLocalityLength)]
        public string SurroundingLocality { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxRoadConditionLength, MinimumLength = CaseIncidentDetailConsts.MinRoadConditionLength)]
        public string RoadCondition { get; set; }

        [StringLength(CaseIncidentDetailConsts.MaxWeatherConditionLength, MinimumLength = CaseIncidentDetailConsts.MinWeatherConditionLength)]
        public string WeatherCondition { get; set; }

        public Guid? CircumstancesFileUpload { get; set; }

        public string CircumstancesFileUploadToken { get; set; }

        public int RegisterId { get; set; }

    }
}