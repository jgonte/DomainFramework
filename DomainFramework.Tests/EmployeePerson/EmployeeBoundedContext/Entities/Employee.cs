using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class Employee : Person
    {
        public DateTime HireDate { get; set; }

    }
}