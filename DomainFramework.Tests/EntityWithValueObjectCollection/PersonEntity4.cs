using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class PersonEntity4 : Entity<int?>
    {
        public string FirstName { get; set; }

        public List<Phone> Phones { get; set; }
    }
}