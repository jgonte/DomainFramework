using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class StudentInputDto : IInputDataTransferObject
    {
        public string FirstName { get; set; }

        public void Validate(ValidationResult result)
        {
            FirstName.ValidateNotEmpty(result, nameof(FirstName));

            FirstName.ValidateMaxLength(result, nameof(FirstName), 50);
        }

    }
}