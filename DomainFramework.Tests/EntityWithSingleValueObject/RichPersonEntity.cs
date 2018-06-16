using DomainFramework.Core;

namespace DomainFramework.Tests.EntityWithValueObjects
{
    class RichPersonEntity : Entity<int?>
    {
        public string FirstName { get; internal set; }

        public Money Capital { get; set; }
    }
}