namespace SYMB2C.Foundation.Settings.Services
{
    public interface ISettingsProvider
    {
        T GetSetting<T>(string settingName, T defaultValue = default);
    }
}