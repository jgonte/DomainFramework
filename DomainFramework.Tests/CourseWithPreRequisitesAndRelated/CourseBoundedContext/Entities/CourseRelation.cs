using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CourseRelationId
    {
        public int? Relates_CourseId { get; set; }

        public int? IsRelatedTo_CourseId { get; set; }

    }

    public class CourseRelation : Entity<CourseRelationId>
    {
        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}