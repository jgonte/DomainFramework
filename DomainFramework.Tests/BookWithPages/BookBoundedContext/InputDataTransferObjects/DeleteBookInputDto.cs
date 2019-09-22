using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class DeleteBookInputDto : IInputDataTransferObject
    {
        public int Id { get; set; }

        public void Validate(ValidationResult result)
        {
            Id.ValidateNotZero(result, nameof(Id));
        }

    }
}