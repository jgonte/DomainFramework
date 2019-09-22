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

        public string Name { get; set; }

        public DateTime StartedDateTime { get; set; }

        public void Validate(ValidationResult result)
        {
            ClassId.ValidateNotZero(result, nameof(ClassId));

            StudentId.ValidateNotZero(result, nameof(StudentId));

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 25);

            StartedDateTime.ValidateNotEmpty(result, nameof(StartedDateTime));
        }

    }
}