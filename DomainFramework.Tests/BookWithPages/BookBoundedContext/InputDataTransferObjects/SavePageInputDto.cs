using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class SavePageInputDto : IInputDataTransferObject
    {
        public int? PageId { get; set; }

        public int Index { get; set; }

        public int? BookId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Index.ValidateNotZero(result, nameof(Index));
        }

    }
}