using DomainFramework.Core;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests
{
    public class BookPagesDto : IInputDataTransferObject
    {
        public string Title { get; set; }

        public IEnumerable<PageDto> Pages { get; set; }

        public ValidationResult Validate()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PageDto : IInputDataTransferObject
    {
        public int Index { get; set; }

        public ValidationResult Validate()
        {
            throw new System.NotImplementedException();
        }
    }

}