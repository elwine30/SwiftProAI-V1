using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ThinknInsurTech.Localization
{
    public static class ThinknInsurTechLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ThinknInsurTechConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ThinknInsurTechLocalizationConfigurer).GetAssembly(),
                        "ThinknInsurTech.Localization.ThinknInsurTech"
                    )
                )
            );
        }
    }
}