using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace PersonWithDisciplesAndServants.PersonBoundedContext
{
    public class PersonInputDto : IInputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? LeaderId { get; set; }

        public int? MasterId { get; set; }

        public List<PersonInputDto> Disciples { get; set; } = new List<PersonInputDto>();

        public List<PersonInputDto> Servants { get; set; } = new List<PersonInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Gender.ValidateRequired(result, nameof(Gender));

            foreach (var person in Disciples)
            {
                person.Validate(result);
            }

            foreach (var person in Servants)
            {
                person.Validate(result);
            }
        }

    }
}