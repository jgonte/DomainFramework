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
            ClassId.ValidateNotZero(result, nameof(ClassId));

            StudentId.ValidateNotZero(result, nameof(StudentId));

            StartedDateTime.ValidateNotEmpty(result, nameof(StartedDateTime));
        }

    }
}