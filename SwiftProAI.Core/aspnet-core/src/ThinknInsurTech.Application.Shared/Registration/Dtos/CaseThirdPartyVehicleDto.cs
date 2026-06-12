using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseThirdPartyVehicleDto : EntityDto
    {
        public string VehicleNo { get; set; }

        public string RegisteredOwner { get; set; }

        public string VehicleMake { get; set; }

        public int? VehicleYear { get; set; }

        public string PolicyNo { get; set; }

        public string TypeCover { get; set; }

        public DateTime? CoverStartDate { get; set; }

        public DateTime? CoverEndDate { get; set; }

        public int RegisterId { get; set; }

        public int? CompanyId { get; set; }

        public Guid? DriverCarGrant { get; set; }

        public string DriverCarGrantToken { get; set; }

    }
}