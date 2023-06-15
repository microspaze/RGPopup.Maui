using System.ComponentModel;
using System.Reflection;

namespace RGPopup.Maui.Converters.TypeConverters
{
    public class EasingTypeConverter : TypeConverter
    {
        public new object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                var fieldInfo = typeof(Easing).GetRuntimeFields()?.FirstOrDefault(fi =>
                {
                    if (fi.IsStatic)
                        return fi.Name == value;
                    return false;
                });
                if (fieldInfo != null)
                    return (Easing)fieldInfo.GetValue(null);
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Easing)}");
        }
    }
}
