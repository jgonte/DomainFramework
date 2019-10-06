using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

    }
}