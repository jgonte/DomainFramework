using System;
using System.Collections.Generic;
using System.Text;

namespace DomainFramework.Core.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime CreatedDateTime { get; set; }
    }
}
