using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    public class BookPagesOutputDto : IOutputDataTransferObject
    {
        public int? Id { get; internal set; }

        public string Title { get; set; }

        public IEnumerable<PageOutputDto> Pages { get; set; }
    }

    public class PageOutputDto : IOutputDataTransferObject
    {
        public int? Id { get; internal set; }

        public int Index { get; set; }

        public int BookId { get; internal set; }
    }
}