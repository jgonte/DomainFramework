using DomainFramework.Core;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class QueryCollectionEmployeeEntityLink : QueryCollectionEntityLink<PersonEntity3, PersonEntity3>
    {
        public override void PopulateEntities(IRepositoryContext repositoryContext, PersonEntity3 entity)
        {
            throw new System.NotImplementedException();
        }

        public override async Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, PersonEntity3 entity)
        {
            var repository = (PersonQueryRepository4)repositoryContext.GetQueryRepository(typeof(PersonEntity3));

            var entities = await repository.GetForManagerAsync(entity.Id);

            LinkedEntities = entities.ToList();
        }
    }
}