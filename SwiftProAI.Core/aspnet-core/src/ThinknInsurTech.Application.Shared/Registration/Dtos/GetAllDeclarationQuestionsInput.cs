using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllDeclarationQuestionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string QuestionFilter { get; set; }

        public string OptionTypeFilter { get; set; }

        public string OptionValuesFilter { get; set; }

        public long? OrganizationUnitIdFilter { get; set; }

    }
}