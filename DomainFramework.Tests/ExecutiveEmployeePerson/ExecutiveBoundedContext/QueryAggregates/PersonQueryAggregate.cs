using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int, PersonOutputDto>
    {
        public PersonQueryAggregate() : this(null)
        {
        }

        public PersonQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ExecutiveEmployeePersonConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PersonQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id;

            OutputDto.Name = RootEntity.Name;
        }

    }
}