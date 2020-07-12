using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class PreRequisiteId
    {
        public int RequiredCourseId { get; set; }

        public int CourseId { get; set; }

    }

    public class PreRequisite : Entity<PreRequisiteId>
    {
        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}