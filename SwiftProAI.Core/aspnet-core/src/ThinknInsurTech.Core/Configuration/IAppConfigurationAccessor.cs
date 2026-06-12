using Microsoft.Extensions.Configuration;

namespace ThinknInsurTech.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
