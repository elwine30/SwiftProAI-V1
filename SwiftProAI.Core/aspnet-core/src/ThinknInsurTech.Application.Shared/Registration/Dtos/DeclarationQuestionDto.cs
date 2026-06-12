using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class DeclarationQuestionDto : EntityDto
    {
        public string Question { get; set; }

        public string OptionType { get; set; }

        public string OptionValues { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}