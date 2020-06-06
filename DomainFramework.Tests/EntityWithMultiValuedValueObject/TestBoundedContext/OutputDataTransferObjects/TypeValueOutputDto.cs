using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TypeValueOutputDto : IOutputDataTransferObject
    {
        public TypeValue.DataTypes DataType { get; set; }

        public string Data { get; set; }

    }
}