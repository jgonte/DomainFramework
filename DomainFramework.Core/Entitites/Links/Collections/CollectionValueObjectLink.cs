using System;
using System.Collections.Generic;
using System.Text;

namespace DomainFramework.Core
{
    public class CollectionValueObjectLink<TValueObject>
        where TValueObject : IValueObject
    {
        public List<TValueObject> LinkedValueObjects { get; set; }
    }
}
