using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class DeleteBookInputDto : IInputDataTransferObject
    {
        public int BookId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            BookId.ValidateRequired(result, nameof(BookId));
        }

    }
}