using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    [TypeConverter(typeof(CylinderTypeConverter))]
    public class Cylinder : ValueObject<Cylinder>
    {
        public int? Diameter { get; set; }

        public override bool IsEmpty() => 
            Diameter == default(int?);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Diameter
            };

        public static bool TryParse(string s, out Cylinder result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = new Cylinder
                {
                    Diameter = null
                };
            }

            result = new Cylinder
            {
                Diameter = int.Parse(s)
            };

            return true;
        }

    }
}