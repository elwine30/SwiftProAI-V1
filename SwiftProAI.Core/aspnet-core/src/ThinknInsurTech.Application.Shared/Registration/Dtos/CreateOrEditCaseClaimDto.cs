using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseClaimDto : EntityDto<int?>
    {

        public string CaseNo { get; set; }

        public decimal Total { get; set; }

        public string Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "File Charges must be a positive number")]
        public decimal FileCharges { get; set; }

        [StringLength(CaseClaimConsts.MaxFileChargesRemarkLength, MinimumLength = CaseClaimConsts.MinFileChargesRemarkLength)]
        public string FileChargesRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SD must be a positive number")]
        public decimal SD { get; set; }

        public decimal SearchFee { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hotel Fees must be a positive number")]
        public decimal Hotel { get; set; }

        public bool Fraud { get; set; }

        public decimal FraudAmount { get; set; }

        public string Remarks { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Police Fees must be a positive number")]
        public decimal Police { get; set; }

        public string PoliceRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Air Fare must be a positive number")]
        public decimal AirFare { get; set; }

        public string AirFareRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Chartered Transport Fees must be a positive number")]
        public decimal CharteredTransport { get; set; }

        public string CharteredTransportRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Toll Fees must be a positive number")]
        public decimal Toll { get; set; }

        public string TollRemark { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mileage must be a positive number")]
        public decimal MileageKM { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mileage Unit Price must be a positive number")]
        public decimal MileageUnitPrice { get; set; }

        public decimal MileageTotal { get; set; }

        public int RegisterId { get; set; }

        public int? StatusId { get; set; }

    }
}