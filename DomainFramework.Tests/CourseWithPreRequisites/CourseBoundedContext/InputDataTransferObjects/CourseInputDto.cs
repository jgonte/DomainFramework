using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class CourseInputDto : IInputDataTransferObject
    {
        public int? CourseId { get; set; }

        public string Description { get; set; }

        public PreRequisiteInputDto PreRequisite { get; set; }

        public List<CourseInputDto> Requires { get; set; } = new List<CourseInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            Description.ValidateNotEmpty(result, nameof(Description));

            Description.ValidateMaxLength(result, nameof(Description), 50);

            PreRequisite.Validate(result);

            foreach (var course in Requires)
            {
                course.Validate(result);
            }
        }

    }
}