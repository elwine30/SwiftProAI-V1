using System.Threading.Tasks;

namespace ThinknInsurTech.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
