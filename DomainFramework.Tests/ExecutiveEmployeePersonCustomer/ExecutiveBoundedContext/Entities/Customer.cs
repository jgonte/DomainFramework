using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class Customer : Person
    {
        public int Rating { get; set; }

    }
}