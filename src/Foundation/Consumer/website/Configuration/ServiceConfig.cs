using SYMB2C.Foundation.DependencyInjection;
using SYMB2C.Foundation.Settings.Services;

namespace SYMB2C.Foundation.Consumer.Configuration
{
    [Service(typeof(IServiceConfig))]
    public class ServiceConfig : IServiceConfig
    {
        ISettingsProvider _settingProvider;

        public ServiceConfig(ISettingsProvider settingProvider)
        {
            this._settingProvider = settingProvider;
        }

        public string MilSoftKey => this._settingProvider.GetSetting<string>("milsoft_key");

        public string MilsoftPhoneQuery => this._settingProvider.GetSetting<string>("milsoft_phone_query");

        public string MilSoftUserName => this._settingProvider.GetSetting<string>("milsoft_username");

        public string MilSoftUserPassword => this._settingProvider.GetSetting<string>("milsoft_password");
    }
}