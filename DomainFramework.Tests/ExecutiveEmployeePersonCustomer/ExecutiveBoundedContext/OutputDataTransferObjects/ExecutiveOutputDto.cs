using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class ExecutiveOutputDto : EmployeeOutputDto
    {
        public decimal Bonus { get; set; }

        public AssetOutputDto Asset { get; set; }

    }
}