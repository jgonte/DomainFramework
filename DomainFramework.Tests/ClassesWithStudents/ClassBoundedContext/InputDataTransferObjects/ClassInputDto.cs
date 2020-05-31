using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassInputDto : IInputDataTransferObject
    {
        public string Name { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 100);
        }

    }
}