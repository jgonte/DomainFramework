using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace BookWithPages.BookBoundedContext
{
    public class BookOutputDto : IOutputDataTransferObject
    {
        public string Title { get; set; }

        public Book.Categories Category { get; set; }

        public DateTime DatePublished { get; set; }

        public Guid PublisherId { get; set; }

        public int BookId { get; set; }

        public List<PageOutputDto> Pages { get; set; } = new List<PageOutputDto>();

    }
}