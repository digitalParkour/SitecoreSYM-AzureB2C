using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    /// <summary>
    /// Interface for Solar Account Service.
    /// </summary>
    public interface ISolarAccountService
    {
        /// <summary>
        /// Generate URL for Solar Sity access.
        /// </summary>
        /// <param name="email">Account email address.</param>
        /// <param name="id">Power account id.</param>
        /// <returns>Generated encrypted URL.</returns>
        string GetUrl(string email, PowerAccountID id);
    }
}
