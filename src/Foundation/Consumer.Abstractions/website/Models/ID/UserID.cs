using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Caching;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.ID
{
    /// <summary>
    /// Clear Marker for SYMB2C userID. Just a string value.
    /// </summary>
    /// <remarks>
    /// Profiles created before January 2021 have numbers as UserID
    /// Profiles created after January 2021 will have email address as UserID
    /// It cannot be assumed that UserID is an email address.
    /// </remarks>
    [DebuggerDisplay("UserID: {Value}")]
    [Serializable]
    public class UserID : ICacheableModel, ISerializable
    {
        public UserID(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            this.Value = id.Trim();
        }

        public string Value { get; set; }

        public static implicit operator UserID(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? null : new UserID(id);
        }

        public static implicit operator string(UserID id)
        {
            return id?.Value;
        }

        #region ISerializable

        public UserID(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            this.Value = (string)info.GetValue("props", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("props", this.Value, typeof(string));
        }

        #endregion

        public override int GetHashCode()
        {
            return this.Value?.GetHashCode() ?? 0;
        }

        public string GetCacheableKey()
        {
            return $"userid-{this.Value}";
        }

        public override string ToString()
        {
            return this.Value;
        }

    }
}
