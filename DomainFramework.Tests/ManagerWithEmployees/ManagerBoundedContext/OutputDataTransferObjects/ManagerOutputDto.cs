using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerOutputDto : EmployeeOutputDto
    {
        public string Department { get; set; }

        public IEnumerable<EmployeeOutputDto> Employees { get; set; }

    }
}