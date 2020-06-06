using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace PersonWithSpouseAndDependants.PersonBoundedContext
{
    public class SavePersonInputDto : IInputDataTransferObject
    {
        public int? PersonId { get; set; }

        public string Name { get; set; }

        public int? MarriedPersonId { get; set; }

        public int? ProviderPersonId { get; set; }

        public SavePersonInputDto Spouse { get; set; }

        public List<SavePersonInputDto> Dependants { get; set; } = new List<SavePersonInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Spouse?.Validate(result);

            foreach (var person in Dependants)
            {
                person.Validate(result);
            }
        }

    }
}