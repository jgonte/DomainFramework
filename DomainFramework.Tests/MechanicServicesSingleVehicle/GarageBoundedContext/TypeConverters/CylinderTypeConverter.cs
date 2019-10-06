using System;
using System.ComponentModel;
using System.Globalization;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CylinderTypeConverter : TypeConverter
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
                Cylinder cylinder = null;

                if (Cylinder.TryParse((string)value, out cylinder))
                {
                    return cylinder;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
}