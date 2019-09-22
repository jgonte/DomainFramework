using DomainFramework.Core;

namespace DomainFramework.Tests.Entitites
{
    public abstract class PersonRoleEntity : ContainerEntity<int?, IPersonRole>
    {
        internal PersonRoleEntity(IPersonRole data, int? id = null) : base(data, id)
        {
        }
    }
}