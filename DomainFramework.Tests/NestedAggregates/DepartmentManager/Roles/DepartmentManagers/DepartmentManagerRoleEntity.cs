using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class DepartmentManagerRoleEntity : Entity<int?>
    {
        public int EmployeeRoleId { get; set; }

        public int ManagesDepartmentId { get; set; }
    }
}