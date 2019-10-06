using System;
using System.ComponentModel;
using System.Globalization;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class DoorTypeConverter : TypeConverter
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
                Door door = null;

                if (Door.TryParse((string)value, out door))
                {
                    return door;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
}