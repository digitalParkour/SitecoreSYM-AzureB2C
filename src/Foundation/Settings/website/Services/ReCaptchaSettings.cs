using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Settings.Services
{
    [Service(typeof(IReCaptchaSettings))]
    public class ReCaptchaSettings : BaseSettings, IReCaptchaSettings
    {

        public ReCaptchaSettings(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public string PrivateKey()
        {
            return settingsProvider.GetSetting<string>("GoogleCaptchaPrivateKey");
        }

        public string PublicKey()
        {
            return settingsProvider.GetSetting<string>("GoogleCaptchaPublicKey");
        }
    }
}