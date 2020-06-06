using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

        public PhoneNumberOutputDto CellPhone { get; set; }

    }
}