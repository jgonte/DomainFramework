using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.Tests
{
    class QueryCollectionPageEntityLink : QueryCollectionEntityLink<BookEntity, PageEntity>
    {
        public override void PopulateEntities(IQueryRepository repository, BookEntity entity)
        {
            LinkedEntities = ((PageQueryRepository)repository).GetForBook(entity.Id).ToList();
        }
    }
}