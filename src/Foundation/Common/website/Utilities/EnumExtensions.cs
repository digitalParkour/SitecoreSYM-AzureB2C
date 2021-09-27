using System.ComponentModel;

namespace SYMB2C.Foundation.Common.Utilities
{
    public static class EnumExtension
    {
        public static string GetDescriptionValue(this System.Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            return fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attribs && attribs.Length > 0 ?
                attribs[0].Description :
                null;
        }
    }
}