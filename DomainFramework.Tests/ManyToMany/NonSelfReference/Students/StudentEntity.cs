using DomainFramework.Core;
using System;

namespace DomainFramework.Tests
{
    class StudentEntity : Entity<Guid?>
    {
        public string FirstName { get; set; }
    }
}
