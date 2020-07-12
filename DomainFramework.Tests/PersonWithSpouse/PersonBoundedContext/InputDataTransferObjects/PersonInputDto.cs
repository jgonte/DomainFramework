using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace PersonWithSpouse.PersonBoundedContext
{
    public class PersonInputDto : IInputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? SpouseId { get; set; }

        public PersonInputDto MarriedTo { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Gender.ValidateRequired(result, nameof(Gender));

            MarriedTo?.Validate(result);
        }

    }
}