using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Account
{
    /// <summary>
    /// Properties can contain partial values for wildcard search
    /// </summary>
    public enum FindAccountStatus
    {
        FoundRecords,
        NoRecords,
        TooManyRecords,
        Error
    }
}
