using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class GetExecutiveByIdQueryAggregate : GetByIdQueryAggregate<Executive, int?, ExecutiveOutputDto>
    {
        public GetExecutiveByIdQueryAggregate() : this(null)
        {
        }

        public GetExecutiveByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ExecutiveEmployeePersonConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            ExecutiveQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id.Value;

            OutputDto.Bonus = RootEntity.Bonus;

            OutputDto.HireDate = RootEntity.HireDate;

            OutputDto.Name = RootEntity.Name;
        }

    }
}