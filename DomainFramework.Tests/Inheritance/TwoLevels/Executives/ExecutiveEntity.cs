using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveEntity : Entity<int?>
    {
        public decimal Bonus { get; set; }
    }
}