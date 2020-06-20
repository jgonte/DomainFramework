using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TestEntityOutputDto : IOutputDataTransferObject
    {
        public int TestEntityId { get; set; }

        public string Text { get; set; }

        public IEnumerable<TypeValueOutputDto> TypeValues1 { get; set; }

    }
}