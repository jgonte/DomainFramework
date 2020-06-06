using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TestEntity : Entity<int?>
    {
        public string Text { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}