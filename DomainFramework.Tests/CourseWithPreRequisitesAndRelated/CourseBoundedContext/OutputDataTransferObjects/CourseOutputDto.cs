using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CourseOutputDto : IOutputDataTransferObject
    {
        public int CourseId { get; set; }

        public string Description { get; set; }

        public IEnumerable<CourseOutputDto> Relates { get; set; }

        public IEnumerable<CourseOutputDto> Requires { get; set; }

    }
}