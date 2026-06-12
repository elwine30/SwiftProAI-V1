using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseIncidentDetailDto : EntityDto
    {
        public DateTime TimeFrom { get; set; }

        public DateTime TimeTo { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string DirectionFrom { get; set; }

        public string DirectionTo { get; set; }

        public string DirectionVia { get; set; }

        public string DriverDrivingWith { get; set; }

        public int PassengerNo { get; set; }

        public string PassengerRemarks { get; set; }

        public string SiteOfAccident { get; set; }

        public string TypeOfRoad { get; set; }

        public double WidthOfRoad { get; set; }

        public string CenterDemarcation { get; set; }

        public string DriverPathLeft { get; set; }

        public string DriverPathRight { get; set; }

        public string Circumstances { get; set; }

        public double SpeedPrior { get; set; }

        public string ViewOfRoad { get; set; }

        public string ViewDueTo { get; set; }

        public string Visibility { get; set; }

        public string VisibilityDueTo { get; set; }

        public string SurroundingArea { get; set; }

        public string SurroundingLocality { get; set; }

        public string RoadCondition { get; set; }

        public string WeatherCondition { get; set; }

        public Guid? CircumstancesFileUpload { get; set; }

        public string CircumstancesFileUploadFileName { get; set; }

        public int RegisterId { get; set; }

    }
}