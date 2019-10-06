using System;
using System.ComponentModel;
using System.Globalization;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class AssetTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                Asset asset = null;

                if (Asset.TryParse((string)value, out asset))
                {
                    return asset;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
}