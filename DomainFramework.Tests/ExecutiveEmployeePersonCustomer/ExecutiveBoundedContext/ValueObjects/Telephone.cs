using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class Telephone : ValueObject<Telephone>
    {
        public string Number { get; set; }

        public override bool IsEmpty() => 
            Number == default(string);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Number
            };

    }
}