using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class SaveManagerInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public string Department { get; set; }

        public string Name { get; set; }

        public int? SupervisorId { get; set; }

        public List<EmployeeInputDto> Employees { get; set; } = new List<EmployeeInputDto>();

        public void Validate(ValidationResult result)
        {
            Department.ValidateNotEmpty(result, nameof(Department));

            Department.ValidateMaxLength(result, nameof(Department), 50);

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            foreach (var employee in Employees)
            {
                employee.Validate(result);
            }
        }

    }
}