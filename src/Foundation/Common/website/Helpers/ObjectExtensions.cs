using System.Collections.Specialized;
using System.Reflection;

namespace SYMB2C.Foundation.Common.Helpers
{
    /// <summary>
    /// Extensions for any type of objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts object properties into a name value collection.
        /// </summary>
        /// <param name="obj">Any object.</param>
        /// <returns>NameValueCollection.</returns>
        public static NameValueCollection ToNameValueCollection(this object obj)
        {
            var result = new NameValueCollection();
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if ((property.GetValue(obj, null) != null) && !string.IsNullOrEmpty(property.GetValue(obj, null).ToString()))
                {
                    result.Add(property.Name, property.GetValue(obj, null).ToString());
                }
            }

            return result;
        }
    }
}