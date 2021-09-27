using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Services
{

    [Service(typeof(ISolarAccountSettings))]
    public class SolarAccountSettings : BaseSettings, ISolarAccountSettings
    {

        public SolarAccountSettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }
        public string GetMyMeterUrl()
        {
            return settingsProvider.GetSetting<string>("SolarMyMeterUrl");
        }

        public int GetSsoExpiration()
        {
            return settingsProvider.GetSetting<int>("SsoSolarExpiration", 24);
        }
    }
}