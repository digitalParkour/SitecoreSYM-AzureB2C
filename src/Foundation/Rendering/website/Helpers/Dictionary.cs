using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Globalization;
using Sitecore.XA.Foundation.Multisite;

namespace SYMB2C.Foundation.Rendering.Helpers
{
    public static class SiteDictionary
    {
        public static string Lookup(string key, Item contextItem, string defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(key) || contextItem == null)
            {
                return defaultValue ?? key;
            }

            var language = contextItem.Language;

            var options = TranslateOptions.Default;
            options.Database = contextItem.Database;

            var domain = GetDictionaryDomain(contextItem);

            if (domain.GetFallbackDictionaryDomain() != null)
            {
                options.FallbackDomains = new[] { domain.GetFallbackDictionaryDomain().Name };
            }
            else
            {
                options.SuppressFallback = true;
                options.SuppressDefaultFallback = true;
            }

            return Translate.TextByLanguage(GetDictionaryDomain(contextItem).Name, options, key, language, key, null);
        }

        public static DictionaryDomain GetDictionaryDomain(Item contextItem)
        {
            var multisiteContext = ServiceLocator.ServiceProvider.GetService<IMultisiteContext>();

            string name = string.Empty;
            Item dictionaryItem = multisiteContext.GetDictionaryItem(contextItem);
            if (dictionaryItem != null)
            {
                name = dictionaryItem.Name;
            }

            if (DictionaryDomain.TryParse(name, contextItem.Database, out DictionaryDomain dictionaryDomain))
            {
                return dictionaryDomain;
            }

            return DictionaryDomain.GetDefaultDomain(contextItem.Database);
        }
    }
}