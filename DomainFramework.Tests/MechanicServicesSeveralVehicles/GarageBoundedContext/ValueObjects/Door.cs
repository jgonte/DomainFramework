using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    [TypeConverter(typeof(DoorTypeConverter))]
    public class Door : ValueObject<Door>
    {
        public int? Number { get; set; }

        public override bool IsEmpty() => 
            Number == default(int?);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Number
            };

        public static bool TryParse(string s, out Door result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = new Door
                {
                    Number = null
                };
            }

            result = new Door
            {
                Number = int.Parse(s)
            };

            return true;
        }

    }
}