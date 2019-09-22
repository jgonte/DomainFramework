using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace BookWithPages.BookBoundedContext
{
    public class PageOutputDto : IOutputDataTransferObject
    {
        public int Index { get; set; }

        public int BookId { get; set; }

        public int PageId { get; set; }

    }
}