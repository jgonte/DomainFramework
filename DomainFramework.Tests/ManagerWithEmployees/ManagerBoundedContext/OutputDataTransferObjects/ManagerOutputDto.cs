using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerOutputDto : EmployeeOutputDto
    {
        public string Department { get; set; }

        public List<EmployeeOutputDto> Employees { get; set; } = new List<EmployeeOutputDto>();

    }
}