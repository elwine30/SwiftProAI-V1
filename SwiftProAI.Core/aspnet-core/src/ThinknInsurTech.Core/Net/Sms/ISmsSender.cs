using System.Threading.Tasks;

namespace ThinknInsurTech.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}