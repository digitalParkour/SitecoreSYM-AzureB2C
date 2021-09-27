using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Services
{
    [Service(typeof(IRsaSettings))]
    public class RsaSettings : BaseSettings, IRsaSettings
    {

        public RsaSettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }
        public int GetKeySize()
        {
            return settingsProvider.GetSetting<int>("RsaKeySize", 256);
        }

        public string GetKeyValue()
        {
            return settingsProvider.GetSetting<string>("RsaKeyValue");
        }
    }
}