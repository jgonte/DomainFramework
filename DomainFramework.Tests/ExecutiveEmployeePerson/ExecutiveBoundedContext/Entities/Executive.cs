using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class Executive : Employee
    {
        public decimal Bonus { get; set; }

    }
}