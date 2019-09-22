using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class Person : Entity<int?>
    {
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

        public int? MarriedToPersonId { get; set; }

        public PhoneNumber CellPhone { get; set; }

    }
}