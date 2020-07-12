using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class PreRequisiteId
    {
        public int Requires_CourseId { get; set; }

        public int IsRequiredBy_CourseId { get; set; }

    }

    public class PreRequisite : Entity<PreRequisiteId>
    {
        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}