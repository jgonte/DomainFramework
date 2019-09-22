using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class Employee : Person
    {
        public DateTime HireDate { get; set; }

    }
}