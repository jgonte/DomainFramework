using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class SavePageInputDto : IInputDataTransferObject
    {
        public int Index { get; set; }

        public int BookId { get; set; }

        public int? PageId { get; set; }

        public void Validate(ValidationResult result)
        {
            Index.ValidateNotZero(result, nameof(Index));

            BookId.ValidateNotZero(result, nameof(BookId));
        }

    }
}