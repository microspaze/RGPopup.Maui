using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace RGPopup.Maui.Converters.TypeConverters
{
    public class EasingTypeConverter : TypeConverter
    {
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value != null)
            {
                var fieldInfo = typeof(Easing).GetRuntimeFields()?.FirstOrDefault(fi =>
                {
                    if (fi.IsStatic)
                        return fi.Name == value.ToString();
                    return false;
                });
                if (fieldInfo != null)
                    return fieldInfo.GetValue(null) as Easing;
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Easing)}");
        }
    }
}
