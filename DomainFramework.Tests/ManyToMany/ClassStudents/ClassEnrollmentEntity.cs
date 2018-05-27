using DomainFramework.Core;
using System;

namespace DomainFramework.Tests
{
    class ClassEnrollmentEntityId
    {
        public Guid ClassId { get; set; }

        public Guid StudentId { get; set; }
    }

    /// <summary>
    /// The join entity between class and students
    /// </summary>
    class ClassEnrollmentEntity : Entity<ClassEnrollmentEntityId>
    {
        public DateTime StartedDateTime { get; set; }
    }
}
