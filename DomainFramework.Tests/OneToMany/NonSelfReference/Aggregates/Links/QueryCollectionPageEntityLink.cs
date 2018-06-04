using DomainFramework.Core;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class QueryCollectionPageEntityLink : QueryCollectionEntityLink<BookEntity, PageEntity>
    {
        public override void PopulateEntities(IRepositoryContext repositoryContext, BookEntity entity)
        {
            var repository = (PageQueryRepository)repositoryContext.GetQueryRepository(typeof(PageEntity));

            LinkedEntities = repository.GetForBook(entity.Id).ToList();
        }

        public override Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, BookEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}