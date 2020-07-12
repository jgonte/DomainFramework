using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class SaveExecutiveInputDto : IInputDataTransferObject
    {
        public int ExecutiveId { get; set; }

        public decimal Bonus { get; set; }

        public DateTime HireDate { get; set; }

        public string Name { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Bonus.ValidateRequired(result, nameof(Bonus));

            HireDate.ValidateRequired(result, nameof(HireDate));

            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);
        }

    }
}