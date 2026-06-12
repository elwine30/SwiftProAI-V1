using System.Linq;
using ThinknInsurTech.EntityFrameworkCore;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialDeclarationQuestionCreator
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialDeclarationQuestionCreator(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var q1 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Are you familiar with the route?");
            if (q1 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Are you familiar with the route?",
                        OptionType = "RADIO",
                        OptionValues = "Yes,No",
                        TenantId = 1
                    });
            }

            var q2 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "How was the traffic?");
            if (q2 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "How was the traffic?",
                        OptionType = "SINGLE_LINE",
                        TenantId = 1
                    });
            }

            var q3 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Was the place brightly lit?");
            if (q3 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Was the place brightly lit?",
                        OptionType = "RADIO",
                        OptionValues = "Yes,No",
                        TenantId = 1
                    });
            }

            var q4 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Who are/is your passenger/s?");
            if (q4 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Who are/is your passenger/s?",
                        OptionType = "SINGLE_LINE",
                        TenantId = 1
                    });
            }

            var q5 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Were they on official business?");
            if (q5 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Were they on official business?",
                        OptionType = "RADIO",
                        OptionValues = "Yes,No",
                        TenantId = 1
                    });
            }

            var q6 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Were they injured?");
            if (q6 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Were they injured?",
                        OptionType = "RADIO",
                        OptionValues = "Yes,No",
                        TenantId = 1
                    });
            }

            var q7 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "What was your speed prior to the accident?");
            if (q7 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "What was your speed prior to the accident?",
                        OptionType = "SINGLE_LINE",
                        TenantId = 1
                    });
            }

            var q8 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "When did you first notice the third party vehicle?");
            if (q8 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "When did you first notice the third party vehicle?",
                        OptionType = "SINGLE_LINE",
                        TenantId = 1
                    });
            }

            var q9 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Did you take evasive measures to avoid the collision?");
            if (q9 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Did you take evasive measures to avoid the collision?",
                        OptionType = "RADIO",
                        OptionValues = "Yes,No",
                        TenantId = 1
                    });
            }

            var q10 = _context.DeclarationQuestions.FirstOrDefault(p => p.Question == "Was there any pillion / passenger to the said vehicle?");
            if (q10 == null)
            {
                _context.DeclarationQuestions.Add(
                    new DeclarationQuestion
                    {
                        Question = "Was there any pillion / passenger to the said vehicle?",
                        OptionType = "RADIO",
                        OptionValues = "Yes,No",
                        TenantId = 1
                    });
            }

        }
    }
}
