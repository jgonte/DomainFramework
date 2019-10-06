using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class Executive : Employee
    {
        public decimal Bonus { get; set; }

        public Asset Asset { get; set; }

    }
}