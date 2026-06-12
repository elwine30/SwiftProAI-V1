using System.Collections.Generic;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetCaseDeclarationAnswerForEditOutput
    {
        public List<DeclarationQuestionAnswerDto> DeclarationQuestionAnswerList { get; set; }

        public int QuestionCount { get; set; }

        public int AnswerCount { get; set; }

    }
}