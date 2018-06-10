using DomainFramework.Core;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class QueryCollectionFriendEntityLink : QueryCollectionEntityLink<PersonEntity, PersonEntity>
    {
        public override void PopulateEntities(IRepositoryContext repositoryContext, PersonEntity entity)
        {
            var repository = (PersonQueryRepository3)repositoryContext.GetQueryRepository(typeof(PersonEntity));

            LinkedEntities = repository.GetForPerson(entity.Id).ToList();
        }

        public override async Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, PersonEntity entity)
        {
            var repository = (PersonQueryRepository3)repositoryContext.GetQueryRepository(typeof(PersonEntity));

            var entities = await repository.GetForPersonAsync(entity.Id);

            LinkedEntities = entities.ToList();
        }
    }
}