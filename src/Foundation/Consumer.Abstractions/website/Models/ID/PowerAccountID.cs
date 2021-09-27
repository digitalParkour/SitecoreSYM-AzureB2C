using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Caching;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.ID
{
    /// <summary>
    /// SYMB2C Power Account ID is a specially formatted string:
    /// "{CustomerNumber}-{PremiseNumber}"
    /// Where each piece is expected to be 7 digit zero (left) padded number.
    /// </summary>
    /// <example>
    /// Examples instantiating:
    ///     PowerAccountID id = "1234567-0123456"; // implicit cast
    ///     PowerAccountID id = "1234567-123456"; // implicit cast, zero padding auto-added when not provided
    ///     PowerAccountID id = "12345670123456"; // implicit cast
    ///     var id = new PowerAccountID("1234567","0123456");
    ///     var id = new PowerAccountID("1234567","123456"); // zero padding auto-added when not provided
    ///     var id = new PowerAccountID("1234567-0123456");
    ///     var id = new PowerAccountID("1234567-123456"); // zero padding auto-added when not provided
    ///     var id = new PowerAccountID("12345670123456");
    /// Example usages:
    ///     id.isValid // true if valid format, false if value does not meet expected format ( [7 numeric]-[7 numeric], or [14 numeric])
    ///     id.CustomerNumber // access CustomerNumber directly
    ///     id.PremiseNumber //  access PremiseNumber directly
    ///     id.Value // provides the formatted value, "{CustomerNumber}-{PremiseNumber}" or the message "Invalid ID"
    ///     id.RawValue // provides the unformatted value, "{CustomerNumber}{PremiseNumber}" or the message "Invalid ID"
    /// </example>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [Serializable]
    public class PowerAccountID : IEquatable<PowerAccountID>, ICacheableModel, ISerializable
    {
        public PowerAccountID(string customerNumber, string premiseNumber)
        {
            CustomerNumber = customerNumber?.PadLeft(7, '0');
            PremiseNumber = premiseNumber?.PadLeft(7, '0');
        }

        public PowerAccountID(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }
            id = id.Trim();

            // handle hyphen delimited format
            var split = id.Split('-');
            if (split.Length == 2)
            {
                CustomerNumber = split[0].Trim().PadLeft(7,'0');
                PremiseNumber = split[1].Trim().PadLeft(7,'0');
            }
            else if (id.Length == 14)
            // handle 14 digit format
            {
                var len = Math.Min(7, id.Length);
                CustomerNumber = id.Substring(0, len); // first 7
                PremiseNumber = id.Substring(id.Length - len, len); // last 7
            }
        }

        private string DebuggerDisplay => $"{(IsValid ? "Valid" : "Invalid")}: {CustomerNumber}-{PremiseNumber}";

        public string CustomerNumber { get; set; }

        public string PremiseNumber { get; set; }

        public string Value => IsValid ? $"{CustomerNumber}-{PremiseNumber}" : "Invalid ID";

        public string RawValue => IsValid ? $"{CustomerNumber}{PremiseNumber}" : "Invalid ID";

        public bool IsValid =>
               (CustomerNumber?.Length ?? 0) == 7
            && (PremiseNumber?.Length ?? 0) == 7
            && CustomerNumber.All(char.IsNumber)
            && PremiseNumber.All(char.IsNumber);

        public static implicit operator PowerAccountID(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? null : new PowerAccountID(id);
        }

        public static implicit operator string(PowerAccountID id)
        {
            return id.Value;
        }

        public override string ToString()
        {
            return this.Value;
        }

        #region ISerializable

        public PowerAccountID(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            this.CustomerNumber = (string)info.GetValue("customerNumber", typeof(string));
            this.PremiseNumber = (string)info.GetValue("premiseNumber", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("customerNumber", this.CustomerNumber, typeof(string));
            info.AddValue("premiseNumber", this.PremiseNumber, typeof(string));
        }

        #endregion

        public static bool operator == (PowerAccountID lhs, PowerAccountID rhs)
        {
            // Check for null on left side.
            if (object.ReferenceEquals(lhs, null))
            {
                if (object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator != (PowerAccountID lhs, PowerAccountID rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as PowerAccountID);
        }

        public bool Equals(PowerAccountID other)
        {
            // If parameter is null, return false.
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defisymb2c Equals as reference equality.
            return (CustomerNumber == other.CustomerNumber) && (PremiseNumber == other.PremiseNumber);
        }

        public override int GetHashCode()
        {
            return $"{CustomerNumber}{PremiseNumber}".GetHashCode();
        }

        public string GetCacheableKey()
        {
            return $"customernumber-{CustomerNumber}-premisenumber-{PremiseNumber}-poweraccountid-{Value}";
        }
    }
}
