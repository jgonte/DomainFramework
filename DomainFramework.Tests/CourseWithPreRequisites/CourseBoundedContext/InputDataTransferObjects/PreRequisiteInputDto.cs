using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class PreRequisiteInputDto : IInputDataTransferObject
    {
        public int RequiredCourseId { get; set; }

        public int CourseId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            RequiredCourseId.ValidateNotZero(result, nameof(RequiredCourseId));

            CourseId.ValidateNotZero(result, nameof(CourseId));
        }

    }
}