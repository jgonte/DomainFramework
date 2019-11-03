using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace BookWithPages.BookBoundedContext
{
    public class Book : Entity<int?>
    {
        public enum Categories
        {
            Science,
            Action,
            Mistery,
            Romantic
        }

        public string Title { get; set; }

        /// <summary>
        /// The category of the book
        /// </summary>
        public Categories Category { get; set; }

        /// <summary>
        /// The date the book was published
        /// </summary>
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// The id of the book
        /// </summary>
        public Guid PublisherId { get; set; }

        /// <summary>
        /// Whether the books is printed on paper instead of electronic
        /// </summary>
        public bool IsHardCopy { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}