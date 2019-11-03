using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class SaveBookInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public Book.Categories Category { get; set; }

        public DateTime DatePublished { get; set; }

        public Guid PublisherId { get; set; }

        public bool IsHardCopy { get; set; }

        public List<SavePageInputDto> Pages { get; set; } = new List<SavePageInputDto>();

        public void Validate(ValidationResult result)
        {
            Title.ValidateNotEmpty(result, nameof(Title));

            Title.ValidateMaxLength(result, nameof(Title), 150);

            ((int)Category).ValidateNotZero(result, nameof(Category));

            DatePublished.ValidateNotEmpty(result, nameof(DatePublished));

            PublisherId.ValidateNotEmpty(result, nameof(PublisherId));

            foreach (var page in Pages)
            {
                page.Validate(result);
            }
        }

    }
}