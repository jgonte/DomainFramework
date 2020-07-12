using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
{
    public class SavePersonInputDto : IInputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? SpouseId { get; set; }

        public int? BestFriendId { get; set; }

        public SavePersonInputDto MarriedTo { get; set; }

        public SavePersonInputDto BestFriendOf { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Gender.ValidateRequired(result, nameof(Gender));

            MarriedTo?.Validate(result);

            BestFriendOf?.Validate(result);
        }

    }
}