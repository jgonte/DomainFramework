using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class EmployeeInputDto : IInputDataTransferObject
    {
        public string Name { get; set; }

        public int? SupervisorId { get; set; }

        public void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);
        }

    }
}