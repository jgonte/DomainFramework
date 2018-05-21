using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests.Entitites
{
    class PersonEntity : Entity<int?, Person>
    {
        public List<PersonRoleEntity> PersonRoleEntities = new List<PersonRoleEntity>();

        public PersonEntity()
        {
        }

        internal PersonEntity(Person data, int? id = null) : base(data, id)
        {
        }
    }
}
