using DomainFramework.Core;
using System;

namespace DomainFramework.Tests
{
    class ClassEntity : Entity<Guid?>
    {
        public string Name { get; set; }
    }
}
