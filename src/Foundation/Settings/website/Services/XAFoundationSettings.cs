using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Services
{
    [Service(typeof(IXAFoundationSettings))]
    public class XAFoundationSettings : BaseSettings, IXAFoundationSettings
    {
        private readonly ISettingsProvider settingsProvider;
        public XAFoundationSettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public string GetEnvironment()
        {
            return this.settingsProvider.GetSetting<string>("XA.Foundation.Multisite.Environment");
        }
    }
}