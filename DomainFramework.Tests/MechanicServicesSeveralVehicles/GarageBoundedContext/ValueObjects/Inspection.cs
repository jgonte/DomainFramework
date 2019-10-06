using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    [TypeConverter(typeof(InspectionTypeConverter))]
    public class Inspection : ValueObject<Inspection>
    {
        public DateTime? Date { get; set; }

        public override bool IsEmpty() => 
            Date == default(DateTime?);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Date
            };

        public static bool TryParse(string s, out Inspection result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = new Inspection
                {
                    Date = null
                };
            }

            result = new Inspection
            {
                Date = DateTime.Parse(s)
            };

            return true;
        }

    }
}