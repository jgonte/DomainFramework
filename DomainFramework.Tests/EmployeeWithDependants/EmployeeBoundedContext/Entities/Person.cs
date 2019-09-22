using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class Person : Entity<int?>
    {
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

        public int? ProviderEmployeeId { get; set; }

        public PhoneNumber CellPhone { get; set; }

    }
}