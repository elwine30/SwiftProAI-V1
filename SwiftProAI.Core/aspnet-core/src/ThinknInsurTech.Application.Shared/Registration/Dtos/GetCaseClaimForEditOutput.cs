
namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseClaimForEditOutput
    {
        public CreateOrEditCaseClaimDto CaseClaim { get; set; }

        public string StatusDescription { get; set; }

        public decimal MileageUnitPrice { get; set; }

    }
}