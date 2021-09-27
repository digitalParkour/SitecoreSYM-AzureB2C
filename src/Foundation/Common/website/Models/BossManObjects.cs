using System;
using System.Collections;
using System.Text;

namespace SYMB2C.Foundation.Common.Models
{
    /// <summary>
    /// Used to store key information about a user in memory/session
    /// </summary>
    [Serializable]
    public class SessionObject
    {
        #region Private Properties
        private string _userID;
        private string _lastName;
        private string _firstName;
        private string _middleName;
        private string _emailAddress;
        private DateTime _LoginTS;
        private DateTime _ExpiresTS;
        private char[] _UserType;
        private string _ipAddress;
        private ArrayList _vendorNumbers;
        private string _KubraToken1;
        private string _KubraToken2;
        private string _KubraAccountList;
        private string _KubraEmailAddressSED;
        private string _KubraLastName;
        private string _KubraFirstName;
        private string _KubraPassword;
        private DateTime _AccountUpdated;
        private string _KubraTokenFiserv;
        private string _PhoneNumber;
        private string _ActiveAccount;
        #endregion

        #region Public Properties
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string LastName
        {
            get { return _lastName; }
        }
        public string FirstName
        {
            get { return _firstName; }
        }
        public string MiddleName
        {
            get { return _middleName; }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
        }
        public DateTime LoginTS
        {
            get { return _LoginTS; }
        }
        public DateTime ExpiresTS
        {
            get { return _ExpiresTS; }
        }
        public char[] UserType
        {
            get { return _UserType; }
        }
        public string IPAddress
        {
            get { return _ipAddress; }
        }
        public ArrayList VendorNumbers
        {
            get { return _vendorNumbers; }
            set { _vendorNumbers = value; }
        }
        public string KubraToken1
        {
            get { return _KubraToken1; }
        }
        public string KubraToken2
        {
            get { return _KubraToken2; }
        }
        public string KubraAccountList
        {
            get { return _KubraAccountList; }
        }
        public string KubraEmailAddressSED
        {
            get { return _KubraEmailAddressSED; }
        }
        public string KubraLastName
        {
            get { return _KubraLastName; }
        }
        public string KubraFirstName
        {
            get { return _KubraFirstName; }
        }
        public string KubraPassword
        {
            get { return _KubraPassword; }
        }

        public DateTime AccountUpdated
        {
            get { return _AccountUpdated; }
        }

        public string KubraTokenFiserv
        {
            get { return _KubraTokenFiserv; }
        }

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
        }
        public string ActiveAccount
        {
            get { return _ActiveAccount; }
            set { _ActiveAccount = value; }
        }

        #endregion

        public void CopyUserObject(UserObject uo, double expireIn, string ipAddress)
        {
            _userID = uo.UserID;
            _lastName = uo.LastName;
            _firstName = uo.FirstName;
            _middleName = uo.MiddleName;
            _emailAddress = uo.EmailAddress;
            _UserType = uo.UserType;
            _LoginTS = DateTime.UtcNow;
            _ExpiresTS = DateTime.UtcNow.AddMinutes(expireIn);
            _ipAddress = ipAddress;
            _KubraToken1 = uo.KubraToken1;
            _KubraToken2 = uo.KubraToken2;
            _KubraAccountList = uo.KubraAccountList;
            _KubraEmailAddressSED = uo.KubraEmailAddressSED;
            _KubraFirstName = uo.KubraFirstName;
            _KubraLastName = uo.KubraLastName;
            _KubraPassword = uo.KubraPassword;
            _AccountUpdated = uo.AccountUpdated;
            _KubraTokenFiserv = uo.KubraTokenFiserv;
            _PhoneNumber = uo.ContactNumber;
            _vendorNumbers = new ArrayList();
            if (!string.IsNullOrEmpty(uo.VendorNumbers))
            {
                string[] _vendors = uo.VendorNumbers.Split(',');
                foreach (string _vendor in _vendors)
                {
                    _vendorNumbers.Add(_vendor);
                }
            }
        }

        /// <summary>
        /// Sets the IP Address
        /// </summary>
        /// <param name="IPAddress">IPAddress in format xxx.xxx.xxx.xxx</param>
        public void SetIPAddress(string IPAddress)
        {
            _ipAddress = IPAddress;
        }

        /// <summary>
        /// Update the user info for a user
        /// </summary>
        /// <param name="lastName">Last Name</param>
        /// <param name="firstName">First Name</param>
        /// <param name="middleName">Middle Name</param>
        /// <param name="emailAddress">Email Address</param>
        /// <remarks>
        /// Only updates the fields that are non-null
        /// </remarks>
        public void UpdateUserInfo(string lastName, string firstName, string middleName,
            string emailAddress)
        {
            if (!string.IsNullOrEmpty(lastName)) _lastName = lastName;
            if (!string.IsNullOrEmpty(firstName)) _firstName = firstName;
            if (!string.IsNullOrEmpty(middleName)) _middleName = middleName;
            if (!string.IsNullOrEmpty(emailAddress)) _emailAddress = emailAddress;
        }

        public void UpdateKubraInfo(string strAccountList, string strEmailAddress, string strLastName, string strFirstName)
        {
            if (!string.IsNullOrEmpty(strAccountList)) _KubraAccountList = strAccountList;
            if (!string.IsNullOrEmpty(strEmailAddress)) _KubraEmailAddressSED = strEmailAddress;
            if (!string.IsNullOrEmpty(strLastName)) _KubraLastName = strLastName;
            _KubraFirstName = strFirstName;
        }

        /// <summary>
        /// Adds a vendor to the list 
        /// </summary>
        /// <param name="VendorNumber">Vendor Number</param>
        public void AddVendor(string VendorNumber)
        {
            if (!_vendorNumbers.Contains(VendorNumber)) _vendorNumbers.Add(VendorNumber);
            //TODO- Update to BossMan Consume service
            //using (BossMan bm = new BossMan())
            //{
            //    bm.UpdateVendorRecordArrayList(UserID, VendorNumbers.ToString());
            //}
        }

        /// <summary>
        /// Removes a vendor from the list 
        /// </summary>
        /// <param name="VendorNumber">Vendor Number</param>
        public void DelVendor(string VendorNumber)
        {
            if (_vendorNumbers.Contains(VendorNumber)) _vendorNumbers.Remove(VendorNumber);
            //using (BossMan bm = new BossMan())
            //{
            //    bm.UpdateVendorRecordArrayList(UserID, VendorNumbers.ToString());
            //}
        }

        /// <summary>
        /// Retrieves a comma separated list of Vendor Numbers
        /// </summary>
        /// <returns>Comma Separated list of vendor numbers, null if empty</returns>
        public string GetVendors()
        {
            StringBuilder _sb = new StringBuilder();
            foreach (string _vendor in _vendorNumbers)
            {
                _sb.Append(_vendor + ",");
            }
            if (_sb.Length >= 1)
            {
                return _sb.ToString().Substring(0, _sb.Length - 1);
            }

            return null;
        }

        /// <summary>
        /// Updates the Expires Time 
        /// </summary>
        /// <param name="expireIn">Expire in minutes from now</param>
        public void UpdateExpires(double expireIn)
        {
            _ExpiresTS = DateTime.UtcNow.AddMinutes(expireIn);
        }

        /// <summary>
        /// Updates the UserType CharArray
        /// </summary>
        /// <param name="userType">New UserType</param>
        public void UpdateUserType(char[] userType)
        {
            _UserType = userType;
        }

        public void UpdateToken1(string token1)
        {
            _KubraToken1 = token1;
        }

        public void UpdateToken2(string token2)
        {
            _KubraToken2 = token2;
        }
        /// <summary>
        /// Validates Session to determine if session is expired
        /// </summary>
        /// <param name="IPAddress">IP Address of user</param>
        /// <returns>True if expired, False if not</returns>
        public bool IsExpired()
        {
            bool _results = true;
            if (_ExpiresTS > DateTime.UtcNow)
            {
                _results = false;
                UpdateExpires(20);
            }
            return _results;
        }

        /// <summary>
        /// Sets the user type based on parameters passed
        /// </summary>
        /// <param name="UserType">Starting UserType</param>
        /// <param name="BitToSet">Which Bit to Set</param>
        /// <param name="SetToValue">Set to What value</param>
        /// <returns>Char array of new UserType</returns>
        public char[] SetUserTypeSwitch(int BitToSet, char SetToValue)
        {
            char[] _newUserType = new char[256];

            // copy the existing array (even if less than 256)
            for (int flag = 0; flag < _UserType.Length; flag++)
            {
                _newUserType[flag] = _UserType[flag];
            }

            // check if less than 256 and make that length...
            if (_UserType.Length < 256)
            {
                for (int flag = _UserType.Length; flag < 256; flag++)
                {
                    _newUserType[flag] = '0';
                }
            }

            // set bit in question
            _newUserType[BitToSet] = SetToValue;

            // return the new modified array
            _UserType = _newUserType;
            return _newUserType;
        }
    }
}