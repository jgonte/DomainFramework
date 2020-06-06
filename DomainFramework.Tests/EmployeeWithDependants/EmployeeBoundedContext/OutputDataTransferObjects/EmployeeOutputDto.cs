using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

        public PhoneNumberOutputDto CellPhone { get; set; }

        public IEnumerable<PersonOutputDto> Dependants { get; set; }

    }
}