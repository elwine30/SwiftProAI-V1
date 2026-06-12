using System.Threading.Tasks;

namespace ThinknInsurTech.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}