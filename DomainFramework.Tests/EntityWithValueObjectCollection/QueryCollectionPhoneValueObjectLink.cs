using DomainFramework.Core;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class QueryCollectionPhoneValueObjectLink : QueryCollectionValueObjectLink<PersonEntity4, Phone>
    {
        public override void PopulateValueObjects(IRepositoryContext repositoryContext, PersonEntity4 entity)
        {
            var repository = (PersonQueryRepository5)repositoryContext.GetQueryRepository(typeof(PersonEntity4));

            LinkedValueObjects = repository.GetPhones(entity.Id, user: null).ToList();
        }

        public override Task PopulateValueObjectsAsync(IRepositoryContext repositoryContext, PersonEntity4 entity)
        {
            throw new System.NotImplementedException();
        }
    }
}