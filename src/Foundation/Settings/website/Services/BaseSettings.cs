namespace SYMB2C.Foundation.Settings.Services
{
    public class BaseSettings
    {
        internal ISettingsProvider settingsProvider;

        public BaseSettings(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }
    }
}