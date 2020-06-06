using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

    }
}