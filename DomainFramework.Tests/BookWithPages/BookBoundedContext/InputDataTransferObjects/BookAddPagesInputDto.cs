using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class BookAddPagesInputDto : IInputDataTransferObject
    {
        public int BookId { get; set; }

        public List<SavePageInputDto> Pages { get; set; } = new List<SavePageInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            BookId.ValidateRequired(result, nameof(BookId));

            foreach (var page in Pages)
            {
                page.Validate(result);
            }
        }

    }
}