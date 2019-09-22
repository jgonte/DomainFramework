using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class EmployeeOutputDto : PersonOutputDto
    {
        public DateTime HireDate { get; set; }

    }
}