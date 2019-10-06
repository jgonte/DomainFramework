using System;
using System.ComponentModel;
using System.Globalization;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class InspectionTypeConverter : TypeConverter
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
                Inspection inspection = null;

                if (Inspection.TryParse((string)value, out inspection))
                {
                    return inspection;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
}