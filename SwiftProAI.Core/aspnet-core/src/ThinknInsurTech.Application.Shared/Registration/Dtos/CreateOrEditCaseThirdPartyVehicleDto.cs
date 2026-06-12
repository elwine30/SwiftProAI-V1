using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThinknInsurTech.Common.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseThirdPartyVehicleDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CaseThirdPartyVehicleConsts.MaxVehicleNoLength, MinimumLength = CaseThirdPartyVehicleConsts.MinVehicleNoLength)]
        public string VehicleNo { get; set; }

        [StringLength(CaseThirdPartyVehicleConsts.MaxRegisteredOwnerLength, MinimumLength = CaseThirdPartyVehicleConsts.MinRegisteredOwnerLength)]
        public string RegisteredOwner { get; set; }

        [StringLength(CaseThirdPartyVehicleConsts.MaxVehicleMakeLength, MinimumLength = CaseThirdPartyVehicleConsts.MinVehicleMakeLength)]
        public string VehicleMake { get; set; }

        [Range(CaseThirdPartyVehicleConsts.MinVehicleYearValue, CaseThirdPartyVehicleConsts.MaxVehicleYearValue)]
        public int? VehicleYear { get; set; }

        [StringLength(CaseThirdPartyVehicleConsts.MaxPolicyNoLength, MinimumLength = CaseThirdPartyVehicleConsts.MinPolicyNoLength)]
        public string PolicyNo { get; set; }

        [StringLength(CaseThirdPartyVehicleConsts.MaxTypeCoverLength, MinimumLength = CaseThirdPartyVehicleConsts.MinTypeCoverLength)]
        public string TypeCover { get; set; }

        public DateTime? CoverStartDate { get; set; }

        public DateTime? CoverEndDate { get; set; }

        public int RegisterId { get; set; }

        public int? CompanyId { get; set; }

        public List<string> FileTokens { get; set; }
        public List<FileMetadataDto> FileMetaDataList { get; set; }

        public Guid? DriverCarGrant { get; set; }
        public string DriverCarGrantToken { get; set; }


        // NEW SECTION
        // Uploaded Files
        public FileMetadataDto TPVDetails { get; set; }
        public FileMetadataDto TPVCarGrant { get; set; }

        //To Upload files
        public string TPVDetailsToken { get; set; }
        public string TPVCarGrantToken { get; set; }


    }
}