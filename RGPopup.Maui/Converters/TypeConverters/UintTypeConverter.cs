using System.ComponentModel;

namespace RGPopup.Maui.Converters.TypeConverters
{
    public class UintTypeConverter : TypeConverter
    {
        public new object ConvertFromInvariantString(string value)
        {
            try
            {
                return Convert.ToUInt32(value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Cannot convert {value} into {typeof(uint)}");
            }
        }
    }
}
