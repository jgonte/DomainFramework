using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class QuerySingleSpouseEntityLink : QuerySingleEntityLink<PersonEntity2, PersonEntity2>
    {
        public override void PopulateEntity(IRepositoryContext repositoryContext, PersonEntity2 entity)
        {
            var repository = (PersonQueryRepository2)repositoryContext.GetQueryRepository(typeof(PersonEntity2));

            LinkedEntity = repository.GetForPerson(entity.Id);
        }

        public override Task PopulateEntityAsync(IRepositoryContext repositoryContext, PersonEntity2 entity)
        {
            throw new System.NotImplementedException();
        }
    }
}