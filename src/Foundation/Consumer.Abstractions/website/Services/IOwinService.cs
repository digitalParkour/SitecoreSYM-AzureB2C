namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    /// <summary>
    /// Interface wrapper for Microsoft Owin Context using AzureAdB2C Identity Server for authentication.
    /// </summary>
    public interface IOwinService
    {
        void Challenge(string redirectUri, string authenticationName, string authenticationPolicy);

        void SignOut(string authenticationName);
    }
}
