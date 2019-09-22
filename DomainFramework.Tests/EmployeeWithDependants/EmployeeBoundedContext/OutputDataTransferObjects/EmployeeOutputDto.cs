using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

        public List<PersonOutputDto> Dependants { get; set; } = new List<PersonOutputDto>();

    }
}