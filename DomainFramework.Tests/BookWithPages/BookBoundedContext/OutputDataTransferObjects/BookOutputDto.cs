using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace BookWithPages.BookBoundedContext
{
    public class BookOutputDto : IOutputDataTransferObject
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public Book.Categories Category { get; set; }

        public DateTime DatePublished { get; set; }

        public Guid PublisherId { get; set; }

        public bool IsHardCopy { get; set; }

        public IEnumerable<PageOutputDto> Pages { get; set; }

    }
}