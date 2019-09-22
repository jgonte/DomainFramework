using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace BookWithPages.BookBoundedContext
{
    public class Page : Entity<int?>
    {
        public int Index { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

        public int BookId { get; set; }

    }
}