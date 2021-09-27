using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Services
{
    [Service(typeof(IBasicAuthenticationSettings))]
    public class BasicAuthenticationSettings : BaseSettings, IBasicAuthenticationSettings
    {
        public BasicAuthenticationSettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public string GetUsername()
        {
            return settingsProvider.GetSetting<string>("ba:username");
        }

        public string GetPassword()
        {
            return settingsProvider.GetSetting<string>("ba:password");
        }
    }
}