using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class Dependant : ValueObject<Dependant>
    {
        public string Name { get; set; }

        public override bool IsEmpty() => 
            Name == default(string);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Name
            };

    }
}