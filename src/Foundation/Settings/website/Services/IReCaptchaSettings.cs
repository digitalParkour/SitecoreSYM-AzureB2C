namespace SYMB2C.Foundation.Settings.Services
{
    public interface IReCaptchaSettings
    {
        string PrivateKey();

        string PublicKey();
    }
}