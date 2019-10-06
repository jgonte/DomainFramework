using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class Student : Entity<int?>
    {
        public string FirstName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}