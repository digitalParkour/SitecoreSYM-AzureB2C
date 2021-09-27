using Sitecore.Security;

namespace SYMB2C.Foundation.Account
{
    public class ConsumerProfile : UserProfile
    {
        public const string FirstNameProperty = "FirstName";
        public const string LastNameProperty = "LastName";
        public const string PhoneProperty = "Phone";
        public const string BusinessNameProperty = "BusinessName";
        public const string LegacyUserIdProperty = "LegacyUserId";
        public const string AccountListProperty = "AccountList";
        public const string SelectedAccountIndexProperty = "SelectedAccountIndex";

        #region Profile Properties

        // == Extended Profile Properties ============================================================================

        public virtual string FirstName
        {
            get { return this[FirstNameProperty]; }
            set { this[FirstNameProperty] = value ?? string.Empty; }
        }

        public virtual string LastName
        {
            get { return this[LastNameProperty]; }
            set { this[LastNameProperty] = value ?? string.Empty; }
        }

        public virtual string Phone
        {
            get { return this[PhoneProperty]; }
            set { this[PhoneProperty] = value ?? string.Empty; }
        }

        public virtual string BusinessName
        {
            get { return this[BusinessNameProperty]; }
            set { this[BusinessNameProperty] = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets 1 based index of selected entry from AccountList.
        /// </summary>
        public virtual string SelectedAccountIndex
        {
            get { return this[SelectedAccountIndexProperty]; }
            set { this[SelectedAccountIndexProperty] = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the legacy ID. Accounts migrated (December 2020) to Azure B2C will have a value. New profiles will not.
        /// </summary>
        public virtual string LegacyUserId
        {
            get { return this[LegacyUserIdProperty]; }
            set { this[LegacyUserIdProperty] = value ?? string.Empty; }
        }

        #endregion

    }
}