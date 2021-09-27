namespace SYMB2C.Foundation.Settings.Services
{
    public interface IAzureDirectorySettings
    {
        string GetClientId();
        string GetMetadataAddress();
        string GetPostLogoutRedirectUri();
        string GetRedirectUri();
        bool GetIsPersistent();
    }
}