using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    public class BookEntity : Entity<int?, Book>
    {
        public BookEntity()
        {
        }

        internal BookEntity(Book data, int? id = null) : base(data, id)
        {
        }       
    }
}
