using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class Rating : ValueObject<Rating>
    {
        public DateTime Number { get; set; }

        public override bool IsEmpty() => 
            Number == default(DateTime);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Number
            };

    }
}