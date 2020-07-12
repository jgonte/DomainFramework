using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassEnrollmentInputDto : IInputDataTransferObject
    {
        public int ClassId { get; set; }

        public int StudentId { get; set; }

        public DateTime StartedDateTime { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            ClassId.ValidateRequired(result, nameof(ClassId));

            StudentId.ValidateRequired(result, nameof(StudentId));

            StartedDateTime.ValidateRequired(result, nameof(StartedDateTime));
        }

    }
}