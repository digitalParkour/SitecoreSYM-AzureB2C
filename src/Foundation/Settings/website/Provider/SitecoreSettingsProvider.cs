using SYMB2C.Foundation.DependencyInjection;
using SYMB2C.Foundation.Settings.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Provider
{
    [Service(typeof(ISettingsProvider))]
    public class SitecoreSettingsProvider : ISettingsProvider
    {
        private readonly Sitecore.Abstractions.BaseSettings _sitecoreSettings;

        public SitecoreSettingsProvider()
        {
            _sitecoreSettings = ServiceLocator.ServiceProvider.GetService<Sitecore.Abstractions.BaseSettings>();
        }

        public SitecoreSettingsProvider(Sitecore.Abstractions.BaseSettings baseSettings)
        {
            _sitecoreSettings = baseSettings;
        }

        private object GetSettingInternal<T>(string settingName, T defaultValue)
        {
            if (typeof(bool) == typeof(T))
            {
                return _sitecoreSettings.GetBoolSetting(settingName, bool.Parse(defaultValue.ToString()));
            }

            if (typeof(int) == typeof(T))
            {
                return _sitecoreSettings.GetIntSetting(settingName, int.Parse(defaultValue.ToString()));
            }

            if (typeof(double) == typeof(T))
            {
                return _sitecoreSettings.GetDoubleSetting(settingName, double.Parse(defaultValue.ToString()));
            }

            if (typeof(long) == typeof(T))
            {
                return _sitecoreSettings.GetLongSetting(settingName, long.Parse(defaultValue.ToString()));
            }

            if (typeof(IReadOnlyCollection<string>) == typeof(T))
            {
                var defaultCollection = new ReadOnlyCollection<string>(new List<string>(defaultValue.ToString().Split('|')));
                return _sitecoreSettings.GetPipedListSetting(settingName, defaultCollection);
            }

            if (typeof(double) == typeof(T))
            {
                if (double.TryParse(_sitecoreSettings.GetSetting(settingName), out double doubleParsed))
                {
                    return doubleParsed;
                }

                return defaultValue;
            }

            if (typeof(Guid) == typeof(T))
            {
                if (Guid.TryParse(_sitecoreSettings.GetSetting(settingName), out Guid guidParsed))
                {
                    return guidParsed;
                }

                return defaultValue;
            }

            if (typeof(Regex) == typeof(T))
            {
                var regexPattern = _sitecoreSettings.GetSetting(settingName, defaultValue.ToString());
                return new Regex(regexPattern);
            }

            if (!string.IsNullOrEmpty(_sitecoreSettings.GetSetting(settingName)))
            {
                return _sitecoreSettings.GetSetting(settingName);
            }

            return defaultValue;
        }
        public T GetSetting<T>(string settingName, T defaultValue = default)
        {
            return (T)this.GetSettingInternal(settingName, defaultValue);
        }
    }
}