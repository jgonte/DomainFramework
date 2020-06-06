using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class Employee : Person
    {
        public DateTime HireDate { get; set; }

    }
}