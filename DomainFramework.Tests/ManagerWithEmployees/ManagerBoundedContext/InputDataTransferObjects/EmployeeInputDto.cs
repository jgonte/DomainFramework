using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class EmployeeInputDto : IInputDataTransferObject
    {
        public int? EmployeeId { get; set; }

        public string Name { get; set; }

        public int? SupervisorId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);
        }

    }
}