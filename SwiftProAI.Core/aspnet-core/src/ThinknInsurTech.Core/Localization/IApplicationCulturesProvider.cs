using System.Globalization;

namespace ThinknInsurTech.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}