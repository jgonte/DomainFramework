using DomainFramework.Core;

namespace DomainFramework.Tests.Entitites
{
    class StudentRoleEntity : ContainerEntity<int?, StudentRole>
    {
        public StudentRoleEntity()
        {
        }

        internal StudentRoleEntity(StudentRole data, int? id = null) : base(data, id)
        {
        }
    }
}
