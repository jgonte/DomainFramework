using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassEnrollmentId
    {
        public int? ClassId { get; set; }

        public int? StudentId { get; set; }

    }

    public class ClassEnrollment : Entity<ClassEnrollmentId>
    {
        public string Name { get; set; }

        public DateTime StartedDateTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

    }
}