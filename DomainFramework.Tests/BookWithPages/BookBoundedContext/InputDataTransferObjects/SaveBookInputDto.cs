using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace BookWithPages.BookBoundedContext
{
    public class SaveBookInputDto : IInputDataTransferObject
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public Book.Categories Category { get; set; }

        public DateTime DatePublished { get; set; }

        public Guid PublisherId { get; set; }

        public bool IsHardCopy { get; set; }

        public List<SavePageInputDto> Pages { get; set; } = new List<SavePageInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            Title.ValidateRequired(result, nameof(Title));

            Title.ValidateMaxLength(result, nameof(Title), 150);

            Category.ValidateRequired(result, nameof(Category));

            DatePublished.ValidateRequired(result, nameof(DatePublished));

            PublisherId.ValidateRequired(result, nameof(PublisherId));

            foreach (var page in Pages)
            {
                page.Validate(result);
            }
        }

    }
}