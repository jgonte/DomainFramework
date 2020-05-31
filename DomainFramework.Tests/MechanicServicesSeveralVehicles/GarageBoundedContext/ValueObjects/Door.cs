using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
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

    }
}