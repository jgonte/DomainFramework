using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
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

    }
}