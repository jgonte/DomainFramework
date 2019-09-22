using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeeRoleEntity : Entity<int?>
    {
        //public int PersonId { get; set; } // That is already the id

        public decimal Salary { get; set; }

        /// <summary>
        /// The department s/he works on
        /// </summary>
        public int DepartmentId { get; set; }
    }
}