using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class Manager : Employee
    {
        public string Department { get; set; }

    }
}