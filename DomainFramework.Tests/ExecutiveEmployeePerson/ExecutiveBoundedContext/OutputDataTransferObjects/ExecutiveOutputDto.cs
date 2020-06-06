using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class ExecutiveOutputDto : EmployeeOutputDto
    {
        public decimal Bonus { get; set; }

    }
}