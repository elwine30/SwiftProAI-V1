using System.Threading.Tasks;
using ThinknInsurTech.Security.Recaptcha;

namespace ThinknInsurTech.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
