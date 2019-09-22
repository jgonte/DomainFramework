using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class DepartmentEntity : Entity<int?>
    {
        public string Name { get; set; }
    }
}