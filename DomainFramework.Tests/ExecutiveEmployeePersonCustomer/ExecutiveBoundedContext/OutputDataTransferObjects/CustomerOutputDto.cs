using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class CustomerOutputDto : PersonOutputDto
    {
        public int Rating { get; set; }

    }
}