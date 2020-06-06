using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
     class ManagerEmployeesQueryAggregate : GetByIdQueryAggregate<PersonEntity3, int?, PersonOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<PersonEntity3> GetEmployeesLoadOperation { get; }

        public IEnumerable<PersonEntity3> Employees => GetEmployeesLoadOperation.LinkedEntities;

        public ManagerEmployeesQueryAggregate(DataAccess.RepositoryContext context) : base(context, null)
        {
            GetEmployeesLoadOperation = new GetCollectionLinkedEntityQueryOperation<PersonEntity3>
            {
                //GetLinkedEntities = (repository, entity, user) =>
                //    ((PersonQueryRepository4)repository).GetForManager(RootEntity.Id, user).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository4)repository).GetForManagerAsync(RootEntity.Id);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(
                GetEmployeesLoadOperation
            );
        }

        public override void PopulateDto()
        {
        }
    }
}