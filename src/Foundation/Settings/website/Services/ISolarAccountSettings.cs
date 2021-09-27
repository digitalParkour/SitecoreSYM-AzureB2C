namespace SYMB2C.Foundation.Settings.Services
{
    public interface ISolarAccountSettings 
    {
        string GetMyMeterUrl();

        int GetSsoExpiration();
    }
}