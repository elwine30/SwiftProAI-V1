using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseClaimDto : EntityDto
    {
        public string CaseNo { get; set; }

        public decimal Total { get; set; }

        public string Location { get; set; }

        public decimal FileCharges { get; set; }

        public string FileChargesRemark { get; set; }

        public decimal SD { get; set; }

        public decimal SearchFee { get; set; }

        public decimal Hotel { get; set; }

        public bool Fraud { get; set; }

        public decimal FraudAmount { get; set; }

        public string Remarks { get; set; }

        public decimal Police { get; set; }

        public string PoliceRemark { get; set; }

        public decimal AirFare { get; set; }

        public string AirFareRemark { get; set; }

        public decimal CharteredTransport { get; set; }

        public string CharteredTransportRemark { get; set; }

        public decimal Toll { get; set; }

        public string TollRemark { get; set; }

        public decimal MileageKM { get; set; }

        public decimal MileageUnitPrice { get; set; }

        public decimal MileageTotal { get; set; }

        public int RegisterId { get; set; }

        public int? StatusId { get; set; }

    }
}