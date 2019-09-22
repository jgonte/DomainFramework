using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class Student : Entity<int?>
    {
        public string FirstName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

    }
}