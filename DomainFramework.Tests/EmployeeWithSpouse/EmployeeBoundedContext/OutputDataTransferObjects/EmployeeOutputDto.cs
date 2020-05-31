using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

        public PhoneNumberOutputDto CellPhone { get; set; }

        public PersonOutputDto Spouse { get; set; }

    }
}