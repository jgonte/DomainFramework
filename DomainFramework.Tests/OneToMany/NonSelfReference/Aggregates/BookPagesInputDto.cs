using DomainFramework.Core;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests
{
    public class BookPagesInputDto : IInputDataTransferObject
    {
        public string Title { get; set; }

        public IEnumerable<PageInputDto> Pages { get; set; }
        public int? Id { get; internal set; }

        public void Validate(ValidationResult result)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PageInputDto : IInputDataTransferObject
    {
        public int Index { get; set; }
        public int? Id { get; internal set; }

        public void Validate(ValidationResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}