using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Registration.Dtos
{
    public class DeclarationQuestionAnswerDto : EntityDto
    {
        public string Question { get; set; }

        public string OptionType { get; set; }

        public string OptionValues { get; set; }

        public int QuestionId { get; set; }

        public string Answer { get; set; }

        public int? AnswerId { get; set; }


    }

}
