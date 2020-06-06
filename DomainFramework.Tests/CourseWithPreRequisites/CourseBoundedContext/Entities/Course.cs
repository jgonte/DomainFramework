using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class Course : Entity<int?>
    {
        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}