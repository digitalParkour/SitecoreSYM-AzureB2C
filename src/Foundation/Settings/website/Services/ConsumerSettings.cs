using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Services
{
    [Service(typeof(IConsumerSettings), Lifetime = Lifetime.Singleton)]
    public class ConsumerSettings : BaseSettings, IConsumerSettings
    {
        public ConsumerSettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public string GetBiAuthentication()
        {
            return settingsProvider.GetSetting<string>("ConsumerServices.BI_Authentication");
        }
    }
}