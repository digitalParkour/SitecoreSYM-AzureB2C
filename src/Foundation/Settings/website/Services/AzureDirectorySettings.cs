using SYMB2C.Foundation.DependencyInjection;
using System;

namespace SYMB2C.Foundation.Settings.Services
{
    [Service(typeof(IAzureDirectorySettings))]
    public class AzureDirectorySettings : BaseSettings, IAzureDirectorySettings
    {
        public AzureDirectorySettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public string GetClientId()
        {
            return settingsProvider.GetSetting<string>("ad:ClientId");
        }

        public bool GetIsPersistent()
        {
            return settingsProvider.GetSetting<bool>("ad:IsPersistentUser", false);
        }

        public string GetMetadataAddress()
        {
            return settingsProvider.GetSetting<string>("ad:ADFSDiscoveryDoc");
        }

        public string GetPostLogoutRedirectUri()
        {
            return settingsProvider.GetSetting<string>("ad:PostLogoutRedirectURI");
        }

        public string GetRedirectUri()
        {
            return settingsProvider.GetSetting<string>("ad:RedirectURI");
        }
    }
}