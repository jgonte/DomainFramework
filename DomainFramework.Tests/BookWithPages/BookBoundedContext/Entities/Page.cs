using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace BookWithPages.BookBoundedContext
{
    public class Page : Entity<int?>
    {
        public int Index { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? BookId { get; set; }

    }
}