using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseDeclarationAnswerDto : EntityDto
    {
        public string Answer { get; set; }

        public int RegisterId { get; set; }

        public int QuestionId { get; set; }

    }
}