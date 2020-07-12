using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassEnrollmentId
    {
        public int ClassId { get; set; }

        public int StudentId { get; set; }

    }

    public class ClassEnrollment : Entity<ClassEnrollmentId>
    {
        public DateTime StartedDateTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}