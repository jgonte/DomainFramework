using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class BookAddPagesInputDto : IInputDataTransferObject
    {
        public int Id { get; set; }

        public List<AddPageInputDto> Pages { get; set; } = new List<AddPageInputDto>();

        public void Validate(ValidationResult result)
        {
            Id.ValidateNotZero(result, nameof(Id));

            foreach (var page in Pages)
            {
                page.Validate(result);
            }
        }

    }
}