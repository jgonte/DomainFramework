using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
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

    }
}